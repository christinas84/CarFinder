(function () {
    angular.module('myApp').factory('carSvc', ['$http', '$q', function ($http, $q){
        var service = {};

        service.getYears = function (){
            return $http.post('/api/Car/GetAllYears').then(function(response){
                return response.data;
            });
        }
        service.getMakes = function (selected){
            return $http.post('/api/Car/GetMakesByYear', selected).then(function(response){
                return response.data;
            });
        }
        service.getModels = function (selected) {
            return $http.post('/api/Car/GetModelsByYearMake', selected).then(function (response) {
                return response.data;
            });
        }
        service.getTrims = function (selected){
            return $http.post('/api/Car/getTrimsByYearMakeModel', selected).then(function(response){
                return response.data;
            });
        }
        service.getCars = function (selected){
            return $http.post('/api/Car/getCarsByYearMakeModelTrim', selected).then(function(response){
                return response.data;
            });
        }
        service.getDetails = function (id){
            return $http.post('/api/Car/getCar', {id:id}).then(function (response) {
                return response.data;
            });
}
           return service;
    }]);
    
    
})();