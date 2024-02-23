function Proxify(object, change)
{
    // Örnek Kullanım
    //var obj = Proxify(object, function (object, property, oldValue, newValue) {
    //    console.log('property ' + property + ' changed from ' + oldValue +  ' to ' + newValue);
    //});

    if (object && object.__proxy__)
    {
        return object;
    }

    var proxy = new Proxy(object, {
        get: function (object, name) {
            if (name == '__proxy__')
            {
                return true;
            }
            return object[name];
        },
        set: function (object, name, value) {
            var old = object[name];
            if (value && typeof value == 'object')
            {
                value = proxify(value, change);
            }

            object[name] = value;
            change(object, name, old, value);
        }
    });

    for (var prop in object) {
        if (object.hasOwnProperty(prop) && object[prop] && typeof object[prop] == 'object')
        {
            object[prop] = proxify(object[prop], change);
        }
    }

    return proxy;
}