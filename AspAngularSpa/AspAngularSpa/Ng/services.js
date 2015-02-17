'use strict';

angular.module('services', []).
factory('SessionService', function () {
    'use strict';
    return {
        currentUser: null
    };
}).
factory('AuthenticationService', function ($http, SessionService) {
    'use strict';
    return {
        login: function (user) {
            // this method could be used to call the API and set the user instead of taking it in the function params
            console.log('[login] user:'+user.name +" "+ user.role);
            SessionService.currentUser = user;
        },
        logout: function () {
            //console.log('[logout] user:' + SessionService.currentUser.name + " " + SessionService.currentUser.role);
            SessionService.currentUser = null;
        },
        isLoggedIn: function () {
            return SessionService.currentUser != null;
        },
        isAdmin: function () {
            if (SessionService.currentUser) {
                return SessionService.currentUser.role == 'admin' ? true : false;
            } else {
                return false;
            }
        },
        getUser: function () {
            return SessionService.currentUser;
        }
    };
});