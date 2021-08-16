<?php

namespace Roocket\Cms\Http\Controllers;

use Roocket\Cms\Cms;

class AdminPanelController extends BaseController
{
    public function index()
    {
        return  Cms::listAdmin();
    }
}
