package com.riogames.unitygcmplugin;

import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.util.Log;

public class UnityTimerNotificationBroadcastRecevier extends BroadcastReceiver {
	
	private static final String TAG = UnityTimerNotificationManager.class.getSimpleName();
	
	@Override
	public void onReceive(Context context, Intent intent) {
		Log.v(TAG, "onReceive");
		Log.v(TAG, "Title : " + UnityTimerNotificationManager.TITLE);
		Log.v(TAG, "Text : " + UnityTimerNotificationManager.TEXT);
		
		UnityGCMNotificationManager.showNotification(context, UnityTimerNotificationManager.TITLE, UnityTimerNotificationManager.TEXT, UnityTimerNotificationManager.TEXT);
	}
}
