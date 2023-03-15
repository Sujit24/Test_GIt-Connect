function Utility() {
    this.loadTemplate = function(templateURL) {
        var data = "<h1>Failed to load template : " + templateURL + "</h1>";
        $.ajax({
            async: false,
            url: templateURL,
            success: function(response) {
                data = response;
            }
        });
        return data;
    };
    this.loadTemplateWithParam = function (templateURL, param) {
        var data = "<h1>Failed to load template : " + templateURL + "</h1>";
        $.ajax({
            async: false,
            url: templateURL,
            data: param,
            success: function (response) {
                data = response;
            }
        });
        return data;
    };
    this.intersection = function (intervalCollection, mainCollection, propertyName) {
        return _.filter(intervalCollection, function (obj) {
            var found = false;
            for (var index = 0; index < mainCollection.length; index++) {
                if (mainCollection[index].get(propertyName) == obj[propertyName]) {
                    found = true;
                    break;
                }
            }
            return found;
        });
    };

    this.difference = function (intervalCollection, mainCollection, propertyName) {
        return _.reject(intervalCollection, function (obj) {
            var found = false;
            for (var index = 0; index < mainCollection.length; index++) {
                if (mainCollection[index].get(propertyName) == obj[propertyName]) {
                    found = true;
                    break;
                }
            }
            return found;
        });
    };
}