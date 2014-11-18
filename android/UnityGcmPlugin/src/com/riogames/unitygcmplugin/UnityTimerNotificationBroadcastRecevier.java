package com.riogames.unitygcmplugin;

import android.app.Activity;
import android.content.BroadcastReceiver;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.util.Log;

public class UnityTimerNotificationBroadcastRecevier extends BroadcastReceiver {
	
	private static final String TAG = UnityTimerNotificationManager.class.getSimpleName();
	
	@Override
	public void onReceive(Context context, Intent intent) {
		Log.v(TAG, "onReceive");
		
		SharedPreferences pref = context.getSharedPreferences(UnityTimerNotificationManager.KEY, Activity.MODE_PRIVATE);
		String[] temp = pref.getString(UnityTimerNotificationManager.TIMER, " , ").split(",");
		
		UnityGCMNotificationManager.showNotification(context, temp[0], temp[1], temp[1]);
	}
}
