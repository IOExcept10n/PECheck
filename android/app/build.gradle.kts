plugins {
    alias(libs.plugins.android.application)
    alias(libs.plugins.kotlin.android)
}

android {
    namespace = "com.example.pecheck"
    compileSdk = 35

    defaultConfig {
        applicationId = "com.example.pecheck"
        minSdk = 24
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"

        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_11
        targetCompatibility = JavaVersion.VERSION_11
    }
    kotlinOptions {
        jvmTarget = "11"
    }
    buildFeatures {
        viewBinding = true
    }
}

dependencies {
    // CameraX для встроенного QR-сканера
    implementation("androidx.camera:camera-core:1.4.0-beta03")
    implementation("androidx.camera:camera-camera2:1.4.0-beta03")
    implementation("androidx.camera:camera-lifecycle:1.4.0-beta03")
    implementation("androidx.camera:camera-view:1.4.0-beta03")
    
    // ML Kit для распознавания QR-кодов
    implementation("com.google.mlkit:barcode-scanning:17.2.0")
    
    // QR код генерация
    implementation("com.google.zxing:core:3.5.2")
    
    // Material Design 3
    implementation("com.google.android.material:material:1.12.0")
    
    // ViewPager2 для слайдеров
    implementation("androidx.viewpager2:viewpager2:1.0.0")
    
    // Lottie для анимаций
    implementation("com.airbnb.android:lottie:6.4.0")

    implementation(libs.androidx.core.ktx)
    implementation(libs.androidx.appcompat)
    implementation(libs.material)
    implementation(libs.androidx.activity)
    implementation(libs.androidx.constraintlayout)

    // Fragments
    implementation(libs.androidx.fragment.ktx)

    // Lifecycle
    implementation(libs.androidx.lifecycle.viewmodel.ktx)
    implementation(libs.androidx.lifecycle.runtime.ktx)
    implementation(libs.androidx.lifecycle.viewmodel.android)

    // Coroutines
    implementation(libs.kotlinx.coroutines.android)

    // Networking
    implementation(libs.retrofit)
    implementation(libs.retrofit.converter.moshi)
    implementation(libs.okhttp)
    implementation(libs.okhttp.logging)
    implementation(libs.moshi.kotlin)

    testImplementation(libs.junit)
    androidTestImplementation(libs.androidx.junit)
    androidTestImplementation(libs.androidx.espresso.core)
}