'use strict';

angular.module('myApp.login', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/login', {
    templateUrl: '/ng/login/login.html',
    controller: 'LoginCtrl'
  });
}])

.controller('LoginCtrl', ["$scope", "$http", "$location", "AuthenticationService", function ($scope, $http, $location, AuthenticationService) {
    $scope.login = function () {
        AuthenticationService.login({ name: $scope.credentials.username, role: 'admin' });
        $location.path("/view1");
    }
}]);