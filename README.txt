READ-ME
========

DEPENDENCIES INTERNA PACKAGES:
- TextMeshPro

DEPENDENCIES EXTERNAL PACKAGES:
- Firebase SDK: https://firebase.google.com/docs/unity/setup?hl=pt-br
	Analytics
	Auth
	Crashlytics
	Database
	DynamicLinks
	RemoteConfig
	Functions
- AdMob SDK: https://developers.google.com/admob/unity/ad-placements

RECOMMENDED PACKAGES:
- My Assets: https://assetstore.unity.com/account/assets
- Favourites: https://assetstore.unity.com/account/lists


CONFIGURATION:

Android:
	- Package Name: com.SagittariusGames.SagittariusTemplate
	- Mininum API Player
		Galaxy S3 => Android 4.4.4
		Padrão => Android 6.0
		TV => Android 8.0
	- Colos Space: Gamma
	- Auto Graphics API: on
	- Scripting Backend: Mono (or IL2CPP)
	- Api Compatibility Level: Standard 2.x (or .NET 4.x)
	- Install Location: Automatic
	- Target Architectures: ARMv7

Config Unity Android ??
	SDK: C:\Users\ltrindade\AppData\Local\Android\Sdk

Unity Dashboard: https://dashboard.unity3d.com/landing
Firebase Console:
	https://console.firebase.google.com/?pli=1
	Download json: https://console.firebase.google.com/project/sagittarius-template-3033518/settings/general/android:com.SagittariusGames.SagittariusTemplate
	Copy to: Assets\Plugins\Android\FirebaseApp.androidlib\res\

- Visual Studio Extension:
	GitHub Plugin

- Android Studio:
	Install
		Android SDK Build Tools
		Android SDK Command-line Tools
		Android Emulator
		Android SDK PLataform-Tools
		Google Play Services
		Google USB Driver
		Google Web Driver
		Interl X86 Emulator Accelerator
			
	Emulators:
		cd C:\Users\ltrindade\AppData\Local\Android\Sdk\emulator
		emulator -list-avds
		emulator -avd Pixel_2_API_30
		emulator -avd Nexus_4_API_19
		
	ADB: https://developer.android.com/studio/command-line/adb
		Enable TCP/IP connection on physical devices
			adb tcpip 5555
			adb connect 192.168.15.18:5555
		
		Restart Server
			adb kill-server
			adb start-server
			
		adb devices
		adb -s 192.168.15.18:5555 install Template.apk
		adb -s 192.168.15.18:5555 logcat -d >logcat.txt

OLD:
- Google Play Service: https://github.com/playgameservices/play-games-plugin-for-unity


TESTS:
- Play Maker
- Log Viewer: https://assetstore.unity.com/packages/tools/integration/log-viewer-12047


IMPROVEMENTS:
- quickstart-unity-master
- Unity Remote
- Fibase: autenticação anônima
- Usar namespaces
- guiskin: https://docs.unity3d.com/Manual/class-GUISkin.html



ERRORS:
com.SagittariusGames.SagittariusTemplate/com.google.firebase.auth.api.fallback.service.FirebaseAuthFallbackService} in process ProcessRecord{422ff818 5592:com.SagittariusGames.SagittariusTemplate/u0a78} not same as in map: null
com.SagittariusGames.SagittariusTemplate E/Firebase-Installations: Firebase Installations can not communicate with Firebase server APIs due to invalid configuration. Please update your Firebase initialization process and set valid Firebase options (API key, Project ID, Application ID) when initializing Firebase.
com.SagittariusGames.SagittariusTemplate E/FA: Name is required and can't be empty. Type: event param