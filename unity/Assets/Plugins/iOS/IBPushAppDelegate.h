//
//  IBPushAppDelegate.h
//  Unity-iPhone
//
//  Created by MMiroslav on 1/23/14.
//
//

#import <Foundation/Foundation.h>
#import "InfobipPush.h"
#import "IBPushUtil.h"

@interface UIApplication(IBPush123)

- (void)application:(UIApplication *)application IBPushDidRegisterForRemoteNotificationsWithDeviceToken:(NSData *)devToken;
- (void)application:(UIApplication *)application IBPushDidFailToRegisterForRemoteNotificationsWithError:(NSError *)err;
- (void)application:(UIApplication *)application IBPushDidReceiveRemoteNotification:(NSDictionary *)userInfo;

- (BOOL)application:(UIApplication *)application IBPushDidFinishLaunchingWithOptions:(NSDictionary *)launchOptions;


@end
