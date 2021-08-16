<?php

namespace Roocket\Cms\Http\Middleware;

use Closure;

class Admin
{
    public function handle($request, Closure $next)
    {
        dump('admin middleware');
        return $next($request);
    }
}
