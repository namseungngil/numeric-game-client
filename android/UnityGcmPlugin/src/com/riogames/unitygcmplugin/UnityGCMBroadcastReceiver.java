package com.riogames.unitygcmplugin;

import android.content.Context;
import android.util.Log;

import com.google.android.gcm.GCMBroadcastReceiver;

public class UnityGCMBroadcastReceiver extends GCMBroadcastReceiver {

	private static final String TAG = UnityGCMBroadcastReceiver.class
			.getSimpleName();

	private static final String SERVICE_NAME = "com.riogames.unitygcmplugin.UnityGCMIntentService";

	protected String getGCMIntentServiceClassName(Context context) {
		Log.v(TAG, "getGCMIntentServcieClassName");
		return SERVICE_NAME;
	}
}
