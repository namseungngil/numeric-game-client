package com.riogames.unitygcmplugin;

import java.util.Calendar;
import android.app.Activity;
import android.app.AlarmManager;
import android.app.PendingIntent;
import android.content.Intent;
import android.util.Log;

import com.unity3d.player.UnityPlayer;

public class UnityTimerNotificationManager {
	
	private static final String TAG = UnityTimerNotificationManager.class.getSimpleName();
	public static String TITLE = "";
	public static String TEXT = "";

	public static void register(String str) {
		
		Log.v(TAG, "local notification register");
		
		String[] strArray = str.split(",");
		String date = strArray[0];
		Log.v(TAG, date);
		String title = strArray[1];
		String text = strArray[2];
		
		Activity activity = UnityPlayer.currentActivity;
		
		Calendar calendar = Calendar.getInstance();
		calendar.setTimeInMillis(System.currentTimeMillis());
		
		int temp = Integer.parseInt(date.substring(4, 6));
		Log.v(TAG, "MONTH : " + temp);
//		calendar.set(Calendar.MONTH, temp);
		
		temp = Integer.parseInt(date.substring(0, 4));
		Log.v(TAG, "YEAR : " + temp);
//		calendar.set(Calendar.YEAR, temp);
		
		temp = Integer.parseInt(date.substring(6, 8));
		Log.v(TAG, "DAY_OF_MONTH : " + temp);
		calendar.set(Calendar.DAY_OF_MONTH, temp);
		
		temp = Integer.parseInt(date.substring(8, 10));
		Log.v(TAG, "HOUR_OF_DAY : " + temp);
		calendar.set(Calendar.HOUR_OF_DAY, temp);
		
		temp = Integer.parseInt(date.substring(10, 12));
		Log.v(TAG, "MINUTE : " + temp);
		calendar.set(Calendar.MINUTE, temp);
		
		temp = Integer.parseInt(date.substring(12, 14));
		Log.v(TAG, "SECOND : " + temp);
		calendar.set(Calendar.SECOND, temp);
		
		Intent intent = new Intent(activity, UnityTimerNotificationBroadcastRecevier.class);
		Log.v(TAG, "Title : " + title);
		Log.v(TAG, "Text : " + text);
		TITLE = title;
		TEXT = text;
//		intent.putExtra("title", title);
//		intent.putExtra("text", text);
		PendingIntent pendingIntent = PendingIntent.getBroadcast(activity, 0, intent, 0);
		
		AlarmManager alarmManager = (AlarmManager)activity.getSystemService(Activity.ALARM_SERVICE);
		alarmManager.set(AlarmManager.RTC, calendar.getTimeInMillis(), pendingIntent);
	}
	
	public static void unregister() {
		
		Log.v(TAG, "local notification unregister");
		
		Activity activity = UnityPlayer.currentActivity;
		
		Intent intent = new Intent(activity, UnityTimerNotificationBroadcastRecevier.class);
		PendingIntent pendingIntent = PendingIntent.getBroadcast(activity, 0, intent, 0);
		
		AlarmManager alarmManager = (AlarmManager)activity.getSystemService(Activity.ALARM_SERVICE);
		alarmManager.cancel(pendingIntent);
	}
}
