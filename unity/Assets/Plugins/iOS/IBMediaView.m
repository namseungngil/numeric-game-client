//
//  IBMediaView.m
//  Unity-iPhone
//
//  Created by MMiroslav on 2/3/14.
//
//

#import "IBMediaView.h"

#define UIColorFromRGB(rgbValue) [UIColor colorWithRed:((float)((rgbValue & 0xFF0000) >> 16))/255.0 green:((float)((rgbValue & 0xFF00) >> 8))/255.0 blue:((float)(rgbValue & 0xFF))/255.0 alpha:1.0];


@interface IBMediaView()
+(void)dismissInfobipMediaView:(UIButton *)sender;
@end

@implementation IBMediaView

InfobipMediaView *mediaView = nil;


+(void)addMediaViewWithNotification:(NSString *) notif andCustomization:(NSString *)customiz {
    NSError * e = nil;
    NSDictionary * notification = [NSJSONSerialization JSONObjectWithData:[notif dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    NSDictionary * customization = [NSJSONSerialization JSONObjectWithData:[customiz dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    
    NSString * mediaContent = [notification objectForKey:@"mediaData"];
    NSNumber * x = [customization objectForKey:@"x"];
    NSNumber * y = [customization objectForKey:@"y"];
    NSNumber * width = [customization objectForKey:@"width"];
    NSNumber * height = [customization objectForKey:@"height"];
    NSNumber * shadow = [customization objectForKey:@"shadow"]; //BOOL
    NSNumber * radius = [customization objectForKey:@"radius"]; //int
    
    NSNumber * dismissButtonSize = [customization objectForKey:@"dismissButtonSize"]; //int
    NSNumber * forgroundColorHex = [customization objectForKey:@"forgroundColorHex"]; //hex
    NSNumber * backgroundColorHex = [customization objectForKey:@"backgroundColorHex"]; //hex
    UIColor * forgroundColor = nil;
    UIColor * backgroundColor = nil;
    if (![[NSNull null] isEqual:forgroundColorHex]) {
        forgroundColor = UIColorFromRGB([forgroundColorHex longValue]);
    }
    if (![[NSNull null] isEqual:backgroundColorHex]) {
        backgroundColor = UIColorFromRGB([backgroundColorHex longValue]);
    }
    
    UIView *topView = [[UIApplication sharedApplication] keyWindow].rootViewController.view;
    CGRect frame = CGRectMake([x floatValue], [y floatValue], [width floatValue], [height floatValue]);
    
    mediaView = [[InfobipMediaView alloc] initWithFrame:frame andMediaContent:mediaContent];
    
    
    //set the size od dismiss button
    if(nil != dismissButtonSize){
        if ((nil != backgroundColor) && (nil != forgroundColor)) {
            [mediaView setDismissButtonSize:[dismissButtonSize integerValue]
                        withBackgroundColor:backgroundColor andForegroundColor:forgroundColor];
        } else {
            [mediaView setDismissButtonSize:[dismissButtonSize integerValue]];
        }
    }
    
    // disabe/enable shadow
    if (nil != shadow) {
        mediaView.shadowEnabled = [shadow boolValue];
    }
    
    // corner radius
    if (nil != radius) {
        mediaView.cornerRadius = [radius integerValue];
    } else {
        mediaView.cornerRadius = 0;
    }
    //
    // Add action with selector "yourDismissAction" to the dismiss button inside Infobip Media View
    [mediaView.dismissButton addTarget:self action:@selector(dismissInfobipMediaView:) forControlEvents:UIControlEventTouchUpInside];
    
    
    // display media view
    [topView addSubview:mediaView];
    
}

+(void)dismissInfobipMediaView:(UIButton *)sender {
    // Dismiss the Media View from the super view
    [mediaView removeFromSuperview];
}

@end
