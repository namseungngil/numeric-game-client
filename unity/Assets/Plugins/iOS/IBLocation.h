//
//  IBLocation.h
//  Unity-iPhone
//
//  Created by MMiroslav on 1/29/14.
//
//

#ifndef IB_LOCATION
#define IB_LOCATION

#import "InfobipPush.h"
#import "IBPushUtil.h"

extern "C" {
    void IBEnableLocation();
    void IBDisableLocation();
    bool IBIsLocationEnabled();
    void IBSetBackgroundLocationUpdateModeEnabled(bool enable);
    bool IBBackgroundLocationUpdateModeEnabled();
    void IBSetLocationUpdateTimeInterval(const int seconds);
    int IBLocationUpdateTimeInterval();
    void IBShareLocation(const char *locationCharArray);
    
    // live geo
    void IBEnableLiveGeo();
    void IBDisableLiveGeo();
    bool IBLiveGeoEnabled();
    int IBNumberOfCurrentLiveGeoRegions();
    int IBStopLiveGeoMonitoringForAllRegions();
    void IBSetLiveGeoAccuracy(const double accuracy);
    double IBLiveGeoAccuracy();
}
#endif //IB_LOCATION