window.mudblazorOnHover = {
    addOnHoverListener: function (element, dotNetObject, callbackMethod) {
        element.addEventListener("mouseover", () => {
            dotNetObject.invokeMethodAsync(callbackMethod);
        });
    }
};