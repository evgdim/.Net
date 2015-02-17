'use strict';

angular.module('myApp', [
  'ngRoute',
  'services',
  'myApp.login',
  'myApp.view1',
  'myApp.view2',
  'myApp.version'
]).
config(['$routeProvider', function($routeProvider) {
  $routeProvider.otherwise({redirectTo: '/login'});
}]).
run(["$rootScope", "$location", "AuthenticationService", function ($rootScope, $location, AuthenticationService) {
    $rootScope.$on("$routeChangeStart", function (event, next, current) {
        console.log('[$routeChangeStart]' + AuthenticationService.getUser());
        if (AuthenticationService.isLoggedIn() == false) {
            // no logged user, we should be going to #login
            if ( next.templateUrl == "partials/login.html" ) {
                // already going to #login, no redirect needed
            } else {
                $location.path( "/login" );
            }
        }         
    });
    $rootScope.logout = function () {
        AuthenticationService.logout();
        $location.path("/login");
    }
    $rootScope.isLoggedIn = function () {
        return AuthenticationService.isLoggedIn();
    }
}]);
