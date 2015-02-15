'use strict';

angular.module('myApp.view1', ['ngRoute'])

.config(['$routeProvider', function($routeProvider) {
  $routeProvider.when('/view1', {
    templateUrl: '/ng/view1/view1.html',
    controller: 'View1Ctrl'
  });
}])

.controller('View1Ctrl', ["$scope", "$http", function ($scope, $http) {
    $scope.users = [];
    $scope.$on('$viewContentLoaded', function () {
        $http.get('/api/Users').
          success(function(data, status, headers, config) {
              $scope.users = data;
          }).
          error(function(data, status, headers, config) {
              alert('Error loading users');
          });
        
    });
    $scope.addUser = function () {
        var userObj = { Name: $scope.user.name, EGN: $scope.user.egn };
        var index = $scope.users.push(userObj) - 1;
        $http.post('/api/Users', userObj).
          success(function (data, status, headers, config) {
              $scope.users[index].Id = data.Id;
          }).
          error(function (data, status, headers, config) {
              $scope.users.splice(index, 1);
          });
    };
    $scope.deleteUser = function (e,i) {
        var user = $scope.users[i];
        $scope.users.splice(i,1);
        $http.delete("api/Users/" + user.Id).
            success(function (data, status, headers, config) {
                console.log("User "+data.Name+" deleted.");
            }).
            error(function (data, status, headers, config) {
                $scope.users.push(user);
            });
    }
}]);