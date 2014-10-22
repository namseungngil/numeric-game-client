#import "CWrappedInfobipPush.h"

NSString *const PUSH_SET_USER_ID = @"IBSetUserId_SUCCESS";
NSString *const PUSH_SET_CHANNELS = @"IBSetChannels_SUCCESS";
NSString *const PUSH_GET_CHANNELS = @"IBGetChannels_SUCCESS";
NSString *const PUSH_UNREGISTER =  @"IBUnregister_SUCCESS";
NSString *const PUSH_GET_UNRECEIVED_NOTIFICATION = @"IBGetUnreceivedNotifications_SUCCESS";



void IBSetLogModeEnabled(bool isEnabled, int lLevel) {
    NSLog(@"IBSetLogModeEnabled method");
    InfobipPushLogLevel logLevel = IPPushLogLevelDebug;
    switch (lLevel) {
        case 0: break;
        case 1: logLevel = IPPushLogLevelInfo; break;
        case 2: logLevel = IPPushLogLevelWarn; break;
        case 3: logLevel = IPPushLogLevelError; break;
        default: NSLog(@"IBSetLogModeEnabled-> lLevel > 3");
    }
    
    [InfobipPush setLogModeEnabled:isEnabled withLogLevel:logLevel];
}

bool IBIsLogModeEnabled() {
    return [InfobipPush isLogModeEnabled];
}

void IBSetTimezoneOffsetInMinutes(int offsetMinutes){
    NSLog(@"IBSetTimezoneOffsetInMinutes method");
    [InfobipPush setTimezoneOffsetInMinutes:offsetMinutes];
}

void IBSetTimezoneOffsetAutomaticUpdateEnabled (bool isEnabled){
    NSLog(@"IBSetTimezoneOffsetAutomaticUpdateEnabled method");
    [InfobipPush setTimezoneOffsetAutomaticUpdateEnabled:isEnabled];
}

void IBInitialization(char * appId, char * appSecret){
    NSLog(@"IBInitialization");
    NSString * applicationId = [NSString stringWithFormat:@"%s",appId];
    NSString * applicationSecret = [NSString stringWithFormat:@"%s",appSecret];
    
    [InfobipPush initializeWithAppID:applicationId appSecret:applicationSecret];
	[[UIApplication sharedApplication] registerForRemoteNotificationTypes:(UIRemoteNotificationTypeBadge |
                                                                           UIRemoteNotificationTypeSound |
                                                                           UIRemoteNotificationTypeAlert)];
}

void IBSetUserIdWithNSString(NSString *userId) {
    [InfobipPush setUserID: userId usingBlock:^(BOOL succeeded, NSError *error) {
        if(succeeded) {
            NSLog(@"Setting userID was successful");
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_SET_USER_ID UTF8String], [@"" UTF8String]);
        } else {
            NSLog(@"Setting userID failed");
            NSString * errorCode = [NSString stringWithFormat:@"%u", [error code]];
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_ERROR_HANDLER UTF8String], [errorCode UTF8String]);
        }
    }];
    
}

void IBSetUserId(const char* userId) {
    NSLog(@"IBSetUserId method");
    NSString * userIdString = [NSString stringWithFormat:@"%s",userId];
    IBSetUserIdWithNSString(userIdString);
}

void IBInitializationWithRegistrationData(char * appId, char * appSecret, char * registrationData) {
    IBInitialization(appId, appSecret);
    //    NSLog(@"RegistrationData: %@", registrationData);
    
    NSError *e;
    NSString * regDataString = [NSString stringWithFormat:@"%s", registrationData];
    NSDictionary * regDictionary = [NSJSONSerialization JSONObjectWithData:[regDataString  dataUsingEncoding:NSUTF8StringEncoding]
                                                                   options:NSJSONReadingMutableContainers error:&e];
    
    NSString * userId = [regDictionary objectForKey:@"userId"];
    NSArray * channels = [regDictionary objectForKey:@"channels"];
    
    // prepare channels for AppDelegate
    [IBPushUtil setChannels:channels];
    
    // save UserId for setting later
    [IBPushUtil setUserId:userId];
}

bool IBIsRegistered() {
    return [InfobipPush isRegistered];
}

char* cStringCopy(const char* string) {
    if (string == NULL){
        return NULL;
    }
    
    char* res = (char*)malloc(strlen(string) + 1);
    strcpy(res, string);
    
    return res;
}

char* IBDeviceId() {
    NSString* devId=[InfobipPush deviceID];
    return cStringCopy([devId UTF8String]);
}

char* IBUserId() {
    NSLog(@"IBUserId method");
    NSString* userId = [InfobipPush userID];
    return cStringCopy([userId UTF8String]);
    
}

void IBRegisterToChannels(const char * channelsData) {
    NSError *e;
    NSString * channelsDataString = [NSString stringWithFormat:@"%s", channelsData];
    NSDictionary * channelsDictionary = [NSJSONSerialization JSONObjectWithData:[channelsDataString  dataUsingEncoding:NSUTF8StringEncoding]
                                                                        options:NSJSONReadingMutableContainers error:&e];
    NSNumber * removeExistingChannels = [channelsDictionary objectForKey:@"removeExistingChannels"];
    NSArray * channels = [channelsDictionary objectForKey:@"channels"];
    
    [InfobipPush subscribeToChannelsInBackground:channels removePrevious:[removeExistingChannels boolValue] usingBlock:^(BOOL succeeded, NSError *error) {
        if(succeeded){
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_SET_CHANNELS UTF8String], [@"" UTF8String]);
        } else {
            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];
}

void IBGetRegisteredChannels() {
    [InfobipPush getListOfChannelsInBackgroundUsingBlock:^(BOOL succeeded, NSArray *channels, NSError *error) {
        if (succeeded) {
            //convert channels to json
            NSError * error = 0;
            NSData *channelsJson = [NSJSONSerialization dataWithJSONObject:channels options:0 error:&error];
            NSString *jsonString = [[NSString alloc] initWithData:channelsJson encoding:NSUTF8StringEncoding];
            
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_GET_CHANNELS UTF8String], [jsonString UTF8String]);
        } else {
            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];
    
}

void IBNotifyNotificationOpened(const char * pushIdParam) {
    NSString * pushId = [NSString stringWithFormat:@"%s", pushIdParam];
    //    NSLog(@"PushID: %@", pushId);
    InfobipPushNotification* tmpNotification = [[InfobipPushNotification alloc] init];
    [tmpNotification setMessageID:pushId];
    
    [InfobipPush confirmPushNotificationWasOpened:tmpNotification];
}

void IBSetBadgeNumber(const int badgeNo) {
    [UIApplication sharedApplication].applicationIconBadgeNumber = badgeNo;
}

void IBUnregister(){
    [InfobipPush unregisterFromInfobipPushUsingBlock:^(BOOL succeeded, NSError *error) {
        if(succeeded) {
            NSLog(@"Unregistration was successful");
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_UNREGISTER UTF8String], [@"" UTF8String]);
        } else {
            NSLog(@"Unregistration failed");
            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];
}

void IBGetUnreceivedNotifications() {
    [InfobipPush getListOfUnreceivedNotificationsInBackgroundUsingBlock:^(BOOL succeeded, NSArray *notifications, NSError *error) {
        if (succeeded) {
            NSMutableArray * notificationsArray = [[NSMutableArray alloc] init];
            for (InfobipPushNotification *notification in notifications) {
                [notificationsArray addObject:[IBPushUtil convertNotificationToAndroidFormat:notification]];
            }
            
            NSError * error = 0;
            NSData *notificationsData = [NSJSONSerialization dataWithJSONObject:notificationsArray options:0 error:&error];
            NSString *notificationJson = [[NSString alloc] initWithData:notificationsData encoding:NSUTF8StringEncoding];
            
            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_GET_UNRECEIVED_NOTIFICATION UTF8String], [notificationJson UTF8String]);
        } else {
            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];
}




void IBAddMediaView(const char * notif, const char * customiz) {
    NSString * notificationJson = [NSString stringWithFormat:@"%s", notif];
    NSString * customizationJson = [NSString stringWithFormat:@"%s", customiz];
    [IBMediaView addMediaViewWithNotification:notificationJson andCustomization:customizationJson];
}
