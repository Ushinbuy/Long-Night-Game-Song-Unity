var OpenWindowPlugin = {
    openWindow: function(link)
    {
        var url = Pointer_stringify(link);
        document.onmouseup = function()
        {
        	window.open(url);
        	document.onmouseup = null;
        }
        var url = Pointer_stringify(link);
        document.ontouchend = function()
        {
            window.open(url);
            document.ontouchend = null;
        }        
    },

    usualyClickFunction: function() 
    { 
        document.onmouseup = function()
        {
            document.onmouseup = null;
        }
        document.ontouchend = function()
        {
            document.ontouchend = null;
        }
    }
};

mergeInto(LibraryManager.library, OpenWindowPlugin);