<?php

use Illuminate\Support\Facades\Route;



    Route::get('/adminpanel/index', [\Roocket\Cms\Http\Controllers\AdminPanelController::class, 'index']);
    Route::get('/adminpanel/config', function (){
        return config('cms.url');
    });







