//
//  IBPushAppDelegate.m
//  Unity-iPhone
//
//  Created by MMiroslav on 1/23/14.
//
//

#import "IBPushAppDelegate.h"
#import <objc/runtime.h>


NSString *const PUSH_REGISTER_WITH_DEVICE_TOKEN     = @"IBPushDidRegisterForRemoteNotificationsWithDeviceToken";
NSString *const PUSH_RECEIVE_REMOTE_NOTIFICATION    = @"IBPushDidReceiveRemoteNotification";
NSString *const PUSH_OPEN_REMOTE_NOTIFICATION       = @"IBPushDidOpenRemoteNotification";

IPPushNotificationInfoBlock didReceiveRemoteNotificationBlock = ^void(BOOL succeeded, InfobipPushNotification *notification, NSError *error) {
    if (succeeded) {
        NSDictionary * notificationAndroidStyle = [IBPushUtil convertNotificationToAndroidFormat:notification];
        NSError * err = 0;
        NSData *notificationData = [NSJSONSerialization dataWithJSONObject:notificationAndroidStyle options:0 error:&err];
        NSString *notificationJson = [[NSString alloc] initWithData:notificationData encoding:NSUTF8StringEncoding];
        
        UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_RECEIVE_REMOTE_NOTIFICATION UTF8String], [notificationJson UTF8String]);
    } else {
        [IBPushUtil passErrorCodeToUnity:error];
    }
};

@implementation UIApplication(IBPush)

+(void)load {
    NSLog(@"%s",__FUNCTION__);
    method_exchangeImplementations(class_getInstanceMethod(self, @selector(setDelegate:)), class_getInstanceMethod(self, @selector(setIBPushDelegate:)));
    
    UIApplication *app = [UIApplication sharedApplication];
    NSLog(@"Initializing application: %@, %@", app, app.delegate);
}

- (void) setIBPushDelegate:(id<UIApplicationDelegate>)delegate {
    
    static Class delegateClass = nil;
    
    if(delegateClass == [delegate class])
    {
        [self setIBPushDelegate:delegate];
        return;
    }
    
    delegateClass = [delegate class];
    
    exchangeMethodImplementations(delegateClass, @selector(application:didFinishLaunchingWithOptions:),
                                  @selector(application:IBPushDidFinishLaunchingWithOptions:),
                                  (IMP)IBPushDidFinishLaunchingWithOptions,
                                  "B@:::");
    exchangeMethodImplementations(delegateClass, @selector(application:didRegisterForRemoteNotificationsWithDeviceToken:),
                                  @selector(application:IBPushDidRegisterForRemoteNotificationsWithDeviceToken:),
                                  (IMP)IBPushDidRegisterForRemoteNotificationsWithDeviceToken,
                                  "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didFailToRegisterForRemoteNotificationsWithError:),
                                  @selector(application:IBPushDidFailToRegisterForRemoteNotificationsWithError:),
                                  (IMP)IBPushDidFailToRegisterForRemoteNotificationsWithError,
                                  "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didReceiveRemoteNotification:),
                                  @selector(application:IBPushDidReceiveRemoteNotification:),
                                  (IMP)IBPushDidReceiveRemoteNotification,
                                  "v@:::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didReceiveRemoteNotification:fetchCompletionHandler:),
                                  @selector(application:IBPushDidReceiveRemoteNotificationFetchCompletionHandler:),
                                  (IMP)IBPushDidReceiveRemoteNotificationFetchCompletionHandler,
                                  "v@::::");
    
    exchangeMethodImplementations(delegateClass, @selector(application:didReceiveLocalNotification:),
                                  @selector(application:IBPushDidReceiveLocalNotification:),
                                  (IMP)IBPushDidReceiveLocalNotification,
                                  "v@:::");
    
    [self setIBPushDelegate:delegate];
}

static void exchangeMethodImplementations(Class class, SEL oldMethod, SEL newMethod, IMP impl, const char * signature) {
	Method method = nil;
    //Check whether method exists in the class
	method = class_getInstanceMethod(class, oldMethod);
	
	if (method)
    {
		//if method exists add a new method
		class_addMethod(class, newMethod, impl, signature);
        //and then exchange with original method implementation
		method_exchangeImplementations(class_getInstanceMethod(class, oldMethod), class_getInstanceMethod(class, newMethod));
	}
    else
    {
		//if method does not exist, simply add as orignal method
		class_addMethod(class, oldMethod, impl, signature);
	}
}

void IBPushDidRegisterForRemoteNotificationsWithDeviceToken(id self, SEL _cmd, id application, id devToken){
    NSLog(@"%s",__FUNCTION__);
    
    if ([self respondsToSelector:@selector(application:IBPushDidRegisterForRemoteNotificationsWithDeviceToken:)])
    {
		[self application:application IBPushDidRegisterForRemoteNotificationsWithDeviceToken:devToken];
	}
    
    NSString *token = [NSString stringWithFormat:@"%@", devToken];
    
    UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_REGISTER_WITH_DEVICE_TOKEN UTF8String], [token UTF8String]);
    
//    IPResponseBlock block = ^void(BOOL succeeded, NSError *error) {
//        if (succeeded) {
//            NSLog(@"Register succeeded!");
//            // TODO replace last argument with something useful or remove it
//            UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_REGISTER_WITH_DEVICE_TOKEN UTF8String], [@"" UTF8String]);
//        } else {
//            NSLog(@"IBPush - Register with device token failed.");
//        }
//    };
//    
//    NSArray * channels = [[IBPushUtil channels] copy];
//    if (!channels) {
//        [InfobipPush registerWithDeviceToken:devToken usingBlock:block];
//    } else {
//        [InfobipPush registerWithDeviceToken:devToken toChannels:[channels copy] usingBlock:block];
//    }
    
    [InfobipPush setUserID:[IBPushUtil userId]];
}

void IBPushDidFailToRegisterForRemoteNotificationsWithError(id self, SEL _cmd, id application, id error) {
    NSString * functionName =  [NSString stringWithFormat:@"%s", __FUNCTION__];
    NSLog(@"%@",functionName);
    NSLog(@"%@", error);
    if ([self respondsToSelector:@selector(application:IBPushDidFailToRegisterForRemoteNotificationsWithError:)])
    {
		[self application:application IBPushDidFailToRegisterForRemoteNotificationsWithError:error];
	}
    
    NSString * errorCode = [NSString stringWithFormat:@"%d", [error code]];
    UnitySendMessage([PUSH_SINGLETON UTF8String], [functionName UTF8String], [errorCode UTF8String]);
}

void IBPushDidReceiveRemoteNotification(id self, SEL _cmd, id application, id userInfo) {
    NSString * functionName =  [NSString stringWithFormat:@"%s", __FUNCTION__];
    NSLog(@"%@",functionName);
    
    if ([self respondsToSelector:@selector(application:IBPushDidReceiveRemoteNotification:)])
    {
		[self application:application IBPushDidReceiveRemoteNotification:userInfo];
	}
    
    [InfobipPush didReceiveRemoteNotification:userInfo withAdditionalInformationAndCompletion:didReceiveRemoteNotificationBlock];
}

BOOL IBPushDidFinishLaunchingWithOptions(id self, SEL _cmd, id application, id launchOptions) {
    NSLog(@"%s",__FUNCTION__);
    BOOL result = YES;
	if ([self respondsToSelector:@selector(application:IBPushDidFinishLaunchingWithOptions:)]) {
		result = (BOOL) [self application:application IBPushDidFinishLaunchingWithOptions:launchOptions];
	} else {
		[self applicationDidFinishLaunching:application];
		result = YES;
	}
	
    // TODO
    
	return result;
}

void IBPushDidReceiveLocalNotification(id self, SEL _cmd, id application, id localNotification) {
    NSLog(@"%s", __FUNCTION__);
    [InfobipPush didReceiveLocalNotification:localNotification withCompletion:didReceiveRemoteNotificationBlock];
}

typedef void (^OurCompHandler)(UIBackgroundFetchResult);

void IBPushDidReceiveRemoteNotificationFetchCompletionHandler(id self, SEL _cmd, id application, id notif, id handler) {
    NSLog(@"%s", __FUNCTION__);
    UIApplication * app = (UIApplication *) application;
    InfobipPushNotification * notification = [InfobipPush pushNotificationFromUserInfo:notif];
    [InfobipPush confirmPushNotificationWasReceived:notification];
    
    NSString * event;
    if((app.applicationState == UIApplicationStateActive) || (app.applicationState == UIApplicationStateInactive)) {
        NSLog(@"app.applicationStatus true");
        event = PUSH_OPEN_REMOTE_NOTIFICATION;
        [InfobipPush confirmPushNotificationWasOpened:notification];
    } else {
        NSLog(@"app.applicationStatus false");
        event = PUSH_RECEIVE_REMOTE_NOTIFICATION;
    }
    
    [InfobipPush pushNotificationFromUserInfo:notif getAdditionalInfo:^void(BOOL succeeded, InfobipPushNotification *notification, NSError *error) {
        if (succeeded) {
            NSLog(@"succeeded true");
            NSDictionary * notificationAndroidStyle = [IBPushUtil convertNotificationToAndroidFormat:notification];
            NSError * err = 0;
            NSData *notificationData = [NSJSONSerialization dataWithJSONObject:notificationAndroidStyle options:0 error:&err];
            NSString *notificationJson = [[NSString alloc] initWithData:notificationData encoding:NSUTF8StringEncoding];
            
            UnitySendMessage([PUSH_SINGLETON UTF8String], [event UTF8String], [notificationJson UTF8String]);
        } else {
            NSLog(@"succeeded false");
//            [IBPushUtil passErrorCodeToUnity:error];
        }
    }];

    [UIApplication sharedApplication].applicationIconBadgeNumber = 1;
    [[UIApplication sharedApplication] cancelAllLocalNotifications];
    [UIApplication sharedApplication].applicationIconBadgeNumber = 0;
    
    OurCompHandler completionHandler = (OurCompHandler) handler;
    completionHandler(UIBackgroundFetchResultNewData);
}

@end
