//
//  IBPushManager.h
//  Unity-iPhone
//
//  Created by MMiroslav on 1/27/14.
//
//
#ifndef IB_PUSH_UTIL
#define IB_PUSH_UTIL
#import <Foundation/Foundation.h>
#import "InfobipPush.h"
#import "InfobipMediaView.h"

FOUNDATION_EXPORT NSString *const PUSH_SINGLETON;
FOUNDATION_EXPORT NSString *const PUSH_ERROR_HANDLER;


@interface IBPushUtil : NSObject


+(NSArray *)channels;
+(void)setChannels:(NSArray *)newChannels;

+(NSString *)userId;
+(void)setUserId:(NSString *)newUserId;

+(NSDictionary *)convertNotificationToAndroidFormat:(InfobipPushNotification *)notification;
+(void)passErrorCodeToUnity:(NSError *)err;
-(void)didDismissInfobipMediaView:(InfobipMediaView *)infobipMediaView;

@end

#endif //IB_PUSH_UTIL