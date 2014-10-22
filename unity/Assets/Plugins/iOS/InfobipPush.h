/*
 
 File: InfobipPush.h
 Abstract: Infobip Push iOS library for handling push notifications and push services.
 
 Version: 1.2.0
 
 Minimum iOS version: 5.0
 
 Dependencies:
    CoreLocation.framework
    CoreTelephony.framework
    MapKit.framework
    SystemConfiguration.framework
 
 More information about Infobip Push: http://push.infobip.com
 
 Copyright (C) 2013 Infobip. All Rights Reserved.
 
*/

#import <Availability.h>
#import <UIKit/UIKit.h>
#import "CoreLocation/CoreLocation.h"

#ifndef __IPHONE_5_0
#warning "This project uses features only available in iOS SDK 5.0 and later."
#endif

#ifndef NS_BLOCKS_AVAILABLE
#warning Infobip Push requires blocks
#endif


#pragma mark -
#pragma mark Infobip Enum Declarations

/**
 * Enum for Infobip push error codes
 * @since 1.0.0
 */
typedef enum {
    IPPushNetworkError,                         /* An error when something is wrong with network, either no network or server error */
    IPPushNoMessageIDError,                     /* An error when there's no messageID in push notification and library can't execute an operation without it */
    IPPushJSONError,                            /* An error with JSON encoding/decoding */
    IPPushNoLocationError,                      /* An error when library can't get user location */
    IPPushLocationServiceDisabledError,         /* An error when location services are disabled */
    IPPushNoDeviceTokenError,                   /* An error when there's no device token and library can't execute an operation without it */
    IPPushNotificationChannelsArrayEmptyError,  /* An error when channels array is empty */
    IPPushSilentNotification,                   /* An silent notification was used */
    IPPushUserNotRegisteredError,               /* An error when user is not registered to Infobip Push */
    IPPushUserIDNotDefinedError,                /* An error when userID is not defined */
    IPPushDeviceTokenNotDefinedError,           /* An error when device token from APNS is not defined */
    IPPushChannelsNotDefinedError,              /* An error when channels are not defined */
    IPPushChannelNameEmptyError                 /* An error when channel name is empty string */
} InfobipPushLibraryError;

/**
 * Enum for Infobip push log level types
 * @since 1.0.9
 */
typedef enum {
    IPPushLogLevelError,
    IPPushLogLevelWarn,
    IPPushLogLevelInfo,
    IPPushLogLevelDebug
} InfobipPushLogLevel;


#pragma mark -
#pragma mark Infobip Push Notification object

/**
 * Infobip Push Notification object declaration.
 * @since 1.0.0
 */
@class InfobipPushNotification;


#pragma mark -
#pragma mark Infobip Block Declarations

/**
 * A block declaration for listing subscribed channels.
 * If operation succeeded it takes array of strings, otherwise, an error is produced. Example channels response: @code(@"news",@"music",@"sport")@endcode.
 * @since 1.0.0
 */
typedef void (^IPChannelsListResultBlock)(BOOL succeeded, NSArray *channels, NSError *error);

/**
 * A block declaration for listing unreceived notifications.
 * If operation succeeded it takes array of InfobipPushNotification objects, otherwise, an error is produced.
 * @since 1.0.1
 */
typedef void (^IPUnreceivedNotificationsListResultBlock)(BOOL succeeded, NSArray *notifications, NSError *error);

/**
 * A block declaration for general Infobip Push response.
 * @since 1.0.0
 */
typedef void (^IPResponseBlock)(BOOL succeeded, NSError *error);

/**
 * A block declaration for getting push notification with additional information from Infobip Push service.
 * If operation succeeded it takes an instance of InfobipPushNotification object, otherwise an error is produced.
 * @since 1.0.0
 */
typedef void (^IPPushNotificationInfoBlock)(BOOL succeeded, InfobipPushNotification *notification, NSError *error);



/**
 * Main interface for Infobip Push.
 * Provides all methods for dealing with Infobip Push services.
 * @since 1.0.0
 */
@interface InfobipPush : NSObject

#pragma mark -
#pragma mark Logger methods

/**
 * Set log mode to be enabled or disabled. If enabled, all logs will be written to the console. Default level will be "Info" (IPPushLogLevelInfo).
 * @param isEnabled Bool value to enable or disable log mode
 * @since 1.0.7
 */
+ (void)setLogModeEnabled:(BOOL)isEnabled;

/**
 * Set log mode to be enabled or disabled. If value of parameter isEnabled is equal to NO, then second parameter (log leve) will not be used. Otherwise if value is YES, all logs less and equal to the logLevel value will be written to the console.
 * @param isEnabled Bool value to enable or disable log mode
 * @param logLevel Level of the log which will be used if the log is enabled
 * @since 1.1.0
 */
+ (void)setLogModeEnabled: (BOOL) isEnabled withLogLevel: (InfobipPushLogLevel) logLevel;

/**
 * Check if the log mode is enabled.
 * @return YES if log mode is enabled, otherwise NO is returned
 * @since 1.0.7
 */
+ (BOOL)isLogModeEnabled;


#pragma mark -
#pragma mark Initialization methods

/**
 * Sets the application id and application secret of your application. Get them at https://push.infobip.com
 * @param appID The application id for your Infobip Push application
 * @param appSecret The application secret for your Infobip Push application
 * @since 1.1.0
 *
 * @warning Throws an exception when appID or appSecret are not defined or nil
 */
+ (void)initializeWithAppID:(NSString *)appID appSecret:(NSString *)appSecret;


#pragma mark -
#pragma mark Location methods

/**
 * Set the location update time interval. It is an interval which triggers location updates. Minimum time interval to be set is 5 minutes. Default is 15 minutes.
 * @param timeInterval Desired location update time interval
 * @since 1.1.0
 *
 * @warning Please consider setting the interval as long as possible to prevent battery draining
 */
+ (void)setLocationUpdateTimeInterval:(NSTimeInterval) timeInterval;

/**
 * Get the current location update time interval which is used as a trigger to the location interval updates.
 * @return Current location update time interval
 * @since 1.1.0
 */
+ (NSTimeInterval)locationUpdateTimeInterval;

/**
 * Set the background location update mode to be enabled or disabled. By default, background location updates are disabled.
 * @param isEnabled Bool value to enable or disable background location update
 * @since 1.1.0
 */
+ (void)setBackgroundLocationUpdateModeEnabled:(BOOL)isEnabled;

/**
 * Check if the background location updates are enabled or disabled.
 * @return YES if the background location mode is enabled, otherwise NO is returned
 * @since 1.1.0
 */
+ (BOOL)backgroundLocationUpdateModeEnabled;

/**
 * Check if the location updating is active (either in foreground or background).
 * @return YES if the location update is active, otherwise NO is returned
 * @since 1.1.0
 */
+ (BOOL)locationUpdateActive;

/**
 * Starts location update service (background mode has to be enabled with method setBackgroundLocationUpdateModeEnabled:).
 * By default location update time interval is set to 15 minutes. Minimum time interval is 5 minutes and it can be set with the method setLocationUpdateTimeInterval:.
 * @since 1.1.0
 */
+ (void)startLocationUpdate;

/**
 * Stops location update service (both foreground and background mode).
 * @since 1.1.0
 */
+ (void)stopLocationUpdate;

/**
 * Report defined user location to Infobip Push servers. 
 * Response of the request can be checked with the block statement. Minimum time interval between two user location reports is 5 minutes.
 * @param userLocation Defined user location
 * @since 1.1.0
 */
+ (void)shareLocation:(CLLocation *)userLocation withBlock:(IPResponseBlock)block;

/**
 * Report defined user location to Infobip Push servers. Minimum time interval between two user location reports is 5 minutes.
 * @param userLocation Defined user location
 * @since 1.1.0
 */
+ (void)shareLocation:(CLLocation *)userLocation;


#pragma mark -
#pragma mark Location methods (deprecated)

/**
 * Starts or stops location update service in foreground. Locations will be updated in the intervals of 15 minutes. Use [InfobipPush startLocationUpdate] instead.
 * @param shareLocation Enable or disable location update service
 * @since 1.0.0
 * @see shareUserLocation:setUserLocationUpdateInterval:
 * @deprecated
 */
+ (void)shareUserLocation:(BOOL)shareLocation __deprecated;

/**
 * Starts or stops location update service in foreground with defined time interval. Minimum time interval is 5 minutes. Use [InfobipPush startLocationUpdate] instead
 * @param shareLocation Enable or disable location update service
 * @param timeInterval Desired time interval for location updates
 * @since 1.0.0
 * @warning Please consider setting the interval as long as possible to prevent battery draining
 * @deprecated
 */
+ (void)shareUserLocation:(BOOL)shareLocation setUserLocationUpdateInterval:(NSTimeInterval)timeInterval __deprecated;


#pragma mark -
#pragma mark Live-geo location methods

/**
 * Enable live geo notifications. Live geo notifications are disabled by default.
 * @since 1.2.0
 */
+ (void)enableLiveGeo;

/**
 * Disable live geo notifications. It will also stop all active live geo areas. Live geo notifications are disabled by default.
 * @since 1.2.0
 */
+ (void)disableLiveGeo;

/**
 * Returns status of live geo notifications and live geo areas. If live geo is enabled, YES will be returned, otherwise NO will be returned.
 * @return Status of live geo notifications and live geo areas.
 * @since 1.2.0
 */
+ (BOOL)liveGeoEnabled;

/**
 * Stop all active live geo areas that are beign monitored for the device.
 * @return Number of stopped live geo areas
 * @since 1.2.0
 */
+ (NSInteger)stopLiveGeoMonitoringForAllRegions;

/**
 * Number of active live geo areas that are beign monitored for the device.
 * @return Number of active live geo areas
 * @since 1.2.0
 */
+ (NSInteger)numberOfCurrentLiveGeoRegions;

/**
 * Set the live geo location accuracy that will be used for live geo monitoring. Default live geo accuracy is set to kCLLocationAccuracyHundredMeters.
 * @param accuracy Live geo location accuracy
 * @since 1.2.0
 */
+ (void)setLiveGeoAccuracy:(CLLocationAccuracy)accuracy;

/**
 * Get the current live geo location accuracy that is used for live geo monitoring. Default live geo accuracy is set to kCLLocationAccuracyHundredMeters.
 * @return accuracy Live geo location accuracy
 * @since 1.2.0
 */
+ (CLLocationAccuracy)liveGeoAccuracy;


#pragma mark -
#pragma mark Timezone Offset methods

/**
 * Timezone is updated by default but you can set your own timezone offset in minutes from GMT. If you manually set timezone, automatic checking for timezone changes will be disabled.
 * @param offsetMinutes Timezone offset in minutes
 * @since 1.1.0
 */
+ (void)setTimezoneOffsetInMinutes:(NSInteger)offsetMinutes;

/**
 * Timezone will be automatically updated by default. If you manually set timezone with method setTimezoneOffsetInMinutes:, automatic checking for timezone changes will be disabled.
 * With this method you can change automatic timezone updates to be enabled or disabled according to timezone changes.
 * @param isEnabled Timezone automatic update to be enabled or disabled
 * @since
 @since 1.1.0
 
 */
+ (void)setTimezoneOffsetAutomaticUpdateEnabled:(BOOL)isEnabled;


#pragma mark -
#pragma mark User Information Management methods

/**
 * Set the vaue of user ID. If user ID is not set, an UUID is created and set as user ID.
 * @param userID User ID
 * @since 1.0.8
 */
+ (void)setUserID:(NSString *)userID;

/**
 * Set the value of user ID. If user ID is not set, an UUID is created and set as user ID. Block can be used to check if the update operation was successful.
 * @param userID User ID
 * @since 1.0.8
 */
+ (void)setUserID:(NSString *)userID usingBlock:(IPResponseBlock)block;

/**
 * Get the value of user ID. If user ID is not set, an UUID is created and returned as user ID.
 * @return user ID
 * @since 1.0.8
 */
+ (NSString *)userID;

/**
 * Get the value of device ID.
 * @return Device ID
 * @since 1.0.8
 */
+ (NSString *)deviceID;


#pragma mark -
#pragma mark User Information Management methods (deprecated)

/**
 * Get the value of user ID. Use [InfobipPush userID] instead.
 * @return User ID
 * @since 1.0.0
 * @deprecated
 */
+ (NSString *)getUserID __deprecated;


#pragma mark -
#pragma mark Registration and unregistration methods

/**
 * Register user to Infobip Push services. Registration needs device token from UIApplicationDelegate method application:didRegisterForRemoteNotificationsWithDeviceToken:.
 * @param newDeviceToken Device token for registration from UIApplicationDelegate method
 * @since 1.1.0
 *
 * @code 
   - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
        [InfobipPush registerWithDeviceToken:deviceToken];
   }
 * @endcode
 */
+ (void)registerWithDeviceToken:(NSData *)newDeviceToken;

/**
 * Register user to Infobip Push services. Registration needs device token from UIApplicationDelegate method application:didRegisterForRemoteNotificationsWithDeviceToken:.
 * You can use block to handle successful or failed response.
 * @param newDeviceToken Device token for registration from UIApplicationDelegate method
 * @since 1.1.0
 * 
 * @code
   - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
        [InfobipPush registerWithDeviceToken:deviceToken usingBlock:^(BOOL succeeded, NSError *error) {
            ...
        }];
   }
 * @endcode
 */
+ (void)registerWithDeviceToken:(NSData *)newDeviceToken usingBlock:(IPResponseBlock)block;

/**
 * Register user on defined channels to Infobip Push services. Registration needs device token from UIApplicationDelegate method application:didRegisterForRemoteNotificationsWithDeviceToken:.
 * Channel array must consist of names of channels to which you want to register the user. Empty array means broadcast (all channels).
 * @param newDeviceToken Device token for registration from UIApplicationDelegate method
 * @param channels Array of channel names to register user on. Empty array means broadcast (all channels of application)
 * @since 1.1.0
 *
 * @code
   - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
        [InfobipPush registerWithDeviceToken:deviceToken toChannels:[NSArray arrayWithObjects: @"music", @"sport", @"news", nil]];
   }
 * @endcode
 */
+ (void)registerWithDeviceToken:(NSData *)newDeviceToken toChannels:(NSArray *)channels;

/**
 * Register user on defined channels to Infobip Push services. Registration needs device token from UIApplicationDelegate method application:didRegisterForRemoteNotificationsWithDeviceToken:.
 * Channel array must consist of names of channels to which you want to register the user. Empty array means broadcast (all channels).
 * You can use block to handle successful or failed response.
 * @param newDeviceToken Device token for registration from UIApplicationDelegate method
 * @param channels Array of channel names to register user on. Empty array means broadcast (all channels of application)
 * @since 1.1.0
 *
 * @code
   - (void)application:(UIApplication *)application didRegisterForRemoteNotificationsWithDeviceToken:(NSData *)deviceToken {
        [InfobipPush registerWithDeviceToken:deviceToken toChannels:[NSArray arrayWithObjects: nil] usingBlock:^{
            ...
        }];
   }
 * @endcode
 */
+ (void)registerWithDeviceToken:(NSData *)newDeviceToken toChannels:(NSArray *)channels usingBlock:(IPResponseBlock)block;

/**
 * Unregister user from Infobip Push service.
 * @since 1.0.0
 */
+ (void)unregisterFromInfobipPush;

/**
 * Unregister user from Infobip Push service using block to handle successful or failed response.
 * @since 1.0.6
 */
+ (void)unregisterFromInfobipPushUsingBlock:(IPResponseBlock)block;

/**
 * Status of user registration to Infobip Push services.
 * @return YES if the user is registered, otherwise it returns NO
 * @since 1.0.0
 */
+ (BOOL)isRegistered;


#pragma mark -
#pragma mark Channel Management methods

/**
 * Retrieves a list of registered channels. Successful or failed response can be handled in block statement.
 * @since 1.0.0
 */
+ (void)getListOfChannelsInBackgroundUsingBlock:(IPChannelsListResultBlock)block;

/**
 * Subscribes to channels in with an option to remove previously registered channels. If remove previous is YES then previous registered channels will be removed, otherwise if
 * remove previous is NO then new channels will be added with the previously registered channels.
 * You can use block to handle successful or failed response.
 * @param channels Array of channel names to register user on. Empty array means broadcast (all channels of application)
 * @param removePrevious Indicator for removing or leacing previously registered channels
 * @since 1.0.0
 */
+ (void)subscribeToChannelsInBackground:(NSArray *)channels removePrevious:(BOOL)removePrevious usingBlock:(IPResponseBlock)block;


#pragma mark -
#pragma mark Notification Handling methods

/**
 * Method to handle live geo push notification that are scheduled as local notifications. Method has to be defined inside method application:didReceiveLocalNotification in UIApplicationDelegate.
 * Method will take care of confirmation if the push notification has been received or/and opened. Without the definition of this method, live geo push notifications won't work as defined.
 * You can use block to handle successful with notification message or failed response.
 * @param localNotification Local notification that is received in method application:didReceiveLocalNotification as live geo notification
 * @since 1.2.0
 */
+ (void)didReceiveLocalNotification:(UILocalNotification *)localNotification withCompletion:(IPPushNotificationInfoBlock)block;

/**
 * Method to handle push notification without additional information. Method has to be defined inside method application:didReceiveRemoteNotification in UIApplicationDelegate.
 * Method will take care of confirmation if the push notification has been received or/and opened.
 * You can use block to handle successful with notification message or failed response.
 * @param userInfo User information dictionary that is received in method application:didReceiveRemoteNotification of UIApplicationDelegate
 * @since 1.2.0
 */
+ (void)didReceiveRemoteNotification:(NSDictionary *)userInfo withCompletion:(IPPushNotificationInfoBlock)block;

/**
 * Method to handle push notification with additional information (JSON data, media content, etc). Method has to be defined inside method application:didReceiveRemoteNotification in UIApplicationDelegate.
 * Method will take care of confirmation if the push notification has been received or/and opened.
 * You can use block to handle successful with notification message or failed response.
 * @param userInfo User information dictionary that is received in method application:didReceiveRemoteNotification of UIApplicationDelegate
 * @since 1.2.0
 */
+ (void)didReceiveRemoteNotification:(NSDictionary *)userInfo withAdditionalInformationAndCompletion:(IPPushNotificationInfoBlock)block;

/**
 * Retrieves a list of unreceived notifications. Successful or failed response can be handled in block statement.
 * @since 1.0.1
 */
+ (void)getListOfUnreceivedNotificationsInBackgroundUsingBlock:(IPUnreceivedNotificationsListResultBlock)block;


#pragma mark -
#pragma mark Notification Handling methods (deprecated)

/**
 * Create an InfobipPushNotification object instance based on userInfo dictionary.
 * @param userInfo Dictionary from method application:didReceiveRemoteNotification in UIApplicationDelegate.
 * @return InfobipPushNotification object instance
 * @since 1.0.0
 * @deprecated
 */
+ (InfobipPushNotification *)pushNotificationFromUserInfo:(NSDictionary *)userInfo __deprecated;

/**
 * Create an InfobipPushNotification object instance on userInfo dictionary with additonal information retrieved from Infobip Push service.
 * @param userInfo Dictionary from method application:didReceiveRemoteNotification in UIApplicationDelegate.
 * @since 1.0.0
 * @deprecated
 */
+ (void)pushNotificationFromUserInfo:(NSDictionary *)userInfo getAdditionalInfo:(IPPushNotificationInfoBlock)block __deprecated;

/**
 * Confirms that push notification was received. It's useful if you want to track how many users received the message. 
 * Call this method when you get userInfo from method application:didReceiveRemoteNotification: in UIApplicationDelegate.
 * @param pushNotification InfobipPushNotification object instance
 * @since 1.0.0
 * @deprecated
 */
+ (void)confirmPushNotificationWasReceived:(InfobipPushNotification *)pushNotification __deprecated;

/**
 * Confirms that push notification was opened. It's useful if you want to track how many users opened the message. 
 * Call this method when you display something from a push notification.
 * @param pushNotification InfobipPushNotification object instance
 * @since 1.0.0
 * @deprecated
 */
+ (void)confirmPushNotificationWasOpened:(InfobipPushNotification *)pushNotification __deprecated;

@end


#pragma mark -
#pragma mark Infobip Push Notification object

/**
 * Infobip Push notification model. An model object that's composed of parsed push notification dictionary.
 * @version 1.2.0
 * @since 1.0.0
 */
@interface InfobipPushNotification : NSObject <NSCoding>

/** 
 * @property alert
 * @brief Push notification alert text.
 * @since 1.0.0
 */
@property (nonatomic,retain) NSString *alert;

/** 
 * @property badge
 * @brief Push notification alert badge.
 * @since 1.0.0
 */
@property (nonatomic,retain) NSString *badge;

/** 
 * @property sound
 * @brief Push notification alert sound.
 * @since 1.0.0
 * @warning It can be an empty string if an error occures.
 */
@property (nonatomic,retain) NSString *sound;

/** 
 * @property messageID
 * @brief Infobip Push custom property messageID.
 * Message ID on Infobip Push service. It's used to retrieve notification additional info from Infobip Push service like larger text, url, etc. 
 * This is used because Apple's Push Notification Service supports the maximum of 256 bytes of push notification payload which is often not enough to pass that much information. 
 * It's also used to confirm if the message was receieved or opened.
 * @since 1.0.0
 * @warning It can be empty an string if an error occures
 */
@property (nonatomic,retain) NSString *messageID;

/** 
 * @property messageType
 * @brief Infobip Push custom property messageType. Message type on Infobip Push service. It's basically a mime type to differentiate between message types.
 * @since 1.0.0
 * @warning It can be empty an string if an error occures
 */
@property (nonatomic,retain) NSString *messageType;

/** 
 * @property data
 * @brief Infobip Push custom property data. To retrieve data you should use method didReceiveRemoteNotification:withAdditionalInformationAndCompletion.
 * @since 1.0.0
 * @warning It can be nil if an error occures
 */
@property (nonatomic,retain) NSDictionary *data;

/** 
 * @property additionalInfo
 * @brief Infobip Push custom property additional information. To retrieve additional information you should use method didReceiveRemoteNotification:withAdditionalInformationAndCompletion.
 * @since 1.0.0
 * @warning It can be nil if an error occures
 */
@property (nonatomic,retain) NSDictionary *additionalInfo;

/** 
 * @property mediaContent
 * @brief Infobip Push custom property that contains media content by the media notification. To check if notification contains media content call method isMediaNotification.
 * To retrieve media content you should use method didReceiveRemoteNotification:withAdditionalInformationAndCompletion.
 * @since 1.1.0
 * @warning It can be nil if an error occures
 */
@property (nonatomic,retain) NSString *mediaContent;

/**
 * Check if the notification is the media notification. Notification is Media notification if it contains media content. Media content can be fetched with the propery mediaContent.
 * @since 1.1.0
 */
- (BOOL)isMediaNotification;

@end
