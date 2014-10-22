//
//  IBPushManager.m
//  Unity-iPhone
//
//  Created by MMiroslav on 1/27/14.
//
//

#import "IBPushUtil.h"


NSString *const PUSH_SINGLETON = @"InfobipPushNotifications";
NSString *const PUSH_ERROR_HANDLER = @"IBPushErrorHandler";



@implementation IBPushUtil

// static channels
static NSArray* channels;
+(NSArray *)channels { return channels; }
+(void)setChannels:(NSArray *)newChannels { channels = [newChannels copy]; }

static NSString* userId;
+(NSString *)userId { return userId; }
+(void)setUserId:(NSString *)newUserId { userId = [newUserId copy]; }

+(NSDictionary *)convertNotificationToAndroidFormat:(InfobipPushNotification *)notification {
    NSDictionary * notificationData = [notification data];
    NSMutableDictionary * newNotification = [[NSMutableDictionary alloc] init];
    NSLog(@"Notification: %@", notification);
    
    [newNotification setValue:[notification messageID] forKey:@"notificationId"];
    [newNotification setValue:[notification sound] forKey:@"sound"];
    [newNotification setValue:[notificationData objectForKey:@"url"] forKey:@"url"];
    [newNotification setValue:[notification additionalInfo] forKey:@"additionalInfo"];
    [newNotification setValue:[notification mediaContent] forKey:@"mediaData"];
    [newNotification setValue:[notification alert] forKey:@"title"];
    [newNotification setValue:[notificationData objectForKey:@"message"] forKey:@"message"];
    [newNotification setValue:[notification messageType] forKey:@"mimeType"];
    [newNotification setValue:[notification badge] forKey:@"badge"];
    //    [newNotification setValue:nil forKey:@"vibrate"];
    //    [newNotification setValue:nil forKey:@"light"];
    
    //    return [[NSDictionary alloc] initWithDictionary:newNotification];
    return newNotification;
    
}

+(void)passErrorCodeToUnity:(NSError *)err {
    NSString * errorCode = [NSString stringWithFormat:@"%d", [err code]];
    UnitySendMessage([PUSH_SINGLETON UTF8String], [PUSH_ERROR_HANDLER UTF8String], [errorCode UTF8String]);
}

-(void)didDismissInfobipMediaView:(InfobipMediaView *)infobipMediaView {
    // Unregister as a delegate
    infobipMediaView.delegate = nil;
    
    // Dismiss the Media View from the super view
    [infobipMediaView removeFromSuperview];
}

@end
