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
        isLoggedIn: function () {
            return SessionService.currentUser !== null;
        },
        isAdmin: function () {
            if (SessionService.currentUser) {
                console.log('[isAdmin] user:' + SessionService.currentUser.name + " " + SessionService.currentUser.role);
                return SessionService.currentUser.role == 'admin' ? true : false;
            } else {
                return false;
            }
        }
    };
});