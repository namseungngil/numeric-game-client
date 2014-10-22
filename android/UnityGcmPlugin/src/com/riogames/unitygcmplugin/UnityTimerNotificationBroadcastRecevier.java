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
		// TODO Auto-generated method stub
//		Intent serviceIntent = new Intent(context, UnityTimerNotificationService.class);
//		context.startService(serviceIntent);
		
		UnityGCMNotificationManager.showNotification(context, intent.getStringExtra("title"), intent.getStringExtra("text"), intent.getStringExtra("text"));
	}
}
