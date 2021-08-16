<?php

namespace Roocket\Cms;

use Roocket\Cms\Models\Admin;

class Cms
{
    public static function listAdmin()
    {
        return Admin::select('username')->get();
    }
}
