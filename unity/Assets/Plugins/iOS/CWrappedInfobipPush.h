#ifndef cwrapper_infobip
#define cwrapper_infobip

#import "InfobipPush.h"
#import "InfobipMediaView.h"
#import "IBPushUtil.h"
#import "IBMediaView.h"


extern "C" {
    void IBSetLogModeEnabled(bool isEnabled, int lLevel = 0);
    bool IBIsLogModeEnabled();
    void IBSetTimezoneOffsetInMinutes(int offsetMinutes);
    void IBSetTimezoneOffsetAutomaticUpdateEnabled (bool isEnabled);
    void IBInitialization(char * appId, char * appSecret);
    void IBInitializationWithRegistrationData(char * appId, char * appSecret, char * registrationData);
    bool IBIsRegistered();
    char *IBDeviceId();
    void IBSetUserId(const char* userId);
    char *IBUserId();
    void IBRegisterToChannels(const char * channelsData);
    void IBGetRegisteredChannels();
    void IBNotifyNotificationOpened(const char * pushIdParam) DEPRECATED_ATTRIBUTE;
    void IBSetBadgeNumber(const int badgeNo);
    void IBUnregister();
    void IBGetUnreceivedNotifications();
    void IBAddMediaView(const char * notif, const char * customiz);
}

#endif
