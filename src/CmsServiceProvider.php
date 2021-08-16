<?php

namespace Roocket\Cms;

use Illuminate\Support\ServiceProvider;

class CmsServiceProvider extends ServiceProvider
{
    public function register()
    {
        $this->app->bind('cms', function (){
            return new Cms;
        });

        $this->mergeConfigFrom(__DIR__ . '/Config/main.php', 'cms');
    }

    public function boot()
    {
        require  __DIR__ . '\routes.php';

        $this->loadViewsFrom(__DIR__.'/views' , 'cms');

        $this->app['router']->middleware('admin', \Roocket\Cms\Http\Middleware\Admin::class);

        $this->publishes([
            __DIR__ . '/Config/main.php' => config_path('cms.php'),
            __DIR__ . '/views' => base_path('resources/views/cms'),
            __DIR__ . '/Migrations' => database_path('migrations'),
        ]);

    }
}
