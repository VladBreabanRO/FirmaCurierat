var properties = [
    'boxSizing',
    'width',
    'height',
    'overflowX',
    'overflowY',

    'borderTopWidth',
    'borderRightWidth',
    'borderBottomWidth',
    'borderLeftWidth',

    'paddingTop',
    'paddingRight',
    'paddingBottom',
    'paddingLeft',

    // https://developer.mozilla.org/en-US/docs/Web/CSS/font
    'fontStyle',
    'fontVariant',
    'fontWeight',
    'fontStretch',
    'fontSize',
    'lineHeight',
    'fontFamily',

    'textAlign',
    'textTransform',
    'textIndent',
    'textDecoration',

    'letterSpacing',
    'wordSpacing'
];

var isFirefox = !(window.mozInnerScreenX === null);
var element, mirrorDiv, computed, style;



window.MyJSFunctions = {
    getCaretCoordinates: function (elName) {
        element = document.getElementById(elName);


        mirrorDiv = document.getElementById(element.nodeName + '--mirror-div');
        if (!mirrorDiv) {
            mirrorDiv = document.createElement('div');
            mirrorDiv.id = element.nodeName + '--mirror-div';
            document.body.appendChild(mirrorDiv);
        }

        style = mirrorDiv.style;


        computed = getComputedStyle(element);



        style.whiteSpace = 'pre-wrap';
        if (element.nodeName !== 'INPUT')
            style.wordWrap = 'break-word';

        style.position = 'absolute';
        style.top = element.offsetTop + parseInt(computed.borderTopWidth) + 'px';
        style.left = "400px";
        style.visibility = 'hidden';


        properties.forEach(function (prop) {
            style[prop] = computed[prop];
        });

        if (isFirefox) {
            style.width = parseInt(computed.width) - 2 + 'px'
            if (element.scrollHeight > parseInt(computed.height))
                style.overflowY = 'scroll';
        } else {
            style.overflow = 'hidden';
        }

        mirrorDiv.textContent = element.value.substring(0, element.selectionEnd);

        if (element.nodeName === 'INPUT')
            mirrorDiv.textContent = mirrorDiv.textContent.replace(/\s/g, "\u00a0");

        var span = document.createElement('span');

        span.textContent = element.value.substring(element.selectionEnd) || '.';
        span.style.backgroundColor = "lightgrey";
        mirrorDiv.appendChild(span);


        var a = span.offsetTop + parseInt(computed['borderTopWidth']);
        var b = span.offsetLeft + parseInt(computed['borderLeftWidth']);

        var c = a + "," + b;


        document.body.removeChild(mirrorDiv);

        return c;
    },
    Alert: function (msg) {
        alert(msg);
        return true;
    },
    Log: function (msg) {
        console.log(msg);
        return true;
    },
    GetElementActualTop: function (el) {
        if (document.getElementById(el) !== null) {
            let rect = document.getElementById(el).getBoundingClientRect();
            return rect.top;
        }
        else {
            return 0;
        }
    },
    GetElementActualLeft: function (el) {
        if (document.getElementById(el) !== null) {
            let rect = document.getElementById(el).getBoundingClientRect();
            return rect.left;
        }
        else {
            return 0;
        }
    },
    InsertAtCursor: function (id, myValue) {
      
            // var myField=document.getElement
            //var myField=  document.getElementsByName(id);
            var myField = document.getElementById(id);
           
            if (document.selection) {
                myField.focus();
                sel = document.selection.createRange();
                sel.text = "";
                sel.text = myValue ;

            }
            else if (myField.selectionStart || myField.selectionStart == '0') {
                var startPos = myField.selectionStart;
                var endPos = myField.selectionEnd;
                myField.value = "";
                myField.value = myField.value.substring(0, startPos)
                    +  myValue 
                    + myField.value.substring(endPos, myField.value.length);
            } else {
                myField.value += tagMarkup + myValue + tagMarkup;
            }

          
            var event = new Event('change');
            myField.dispatchEvent(event);
        
      


    },
  
    focusMethod: function (da) {
        var x = document.getElementById("pass");
        x.focus();
    },
    exportSave: function (filename, bytesBase64) {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link); // Needed for Firefox

        link.click();
        document.body.removeChild(link);


    },
    scrollToBottom: function (id) {

        const scrollIntoViewOptions = { behavior: "smooth", block: "center" };
        document.getElementById(id).scrollIntoView(scrollIntoViewOptions);
    }

}


