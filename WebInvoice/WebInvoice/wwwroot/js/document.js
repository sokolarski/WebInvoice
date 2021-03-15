
function disableBankAccount() {
    let paymentType = document.getElementById('paymentType');
    let requareBankAccount = paymentType.options[paymentType.selectedIndex].dataset.requarebankaccount;
    if (requareBankAccount == 'True') {
        document.getElementById('bankAccount').disabled = false;
    }
    else {
        document.getElementById('bankAccount').disabled = true;
    }
}

function tryParseJSON(jsonString) {
    try {
        var o = JSON.parse(jsonString);

        // Handle non-exception-throwing cases:
        // Neither JSON.parse(false) or JSON.parse(1234) throw errors, hence the type-checking,
        // but... JSON.parse(null) returns null, and typeof null === "object",
        // so we must check for that, too. Thankfully, null is falsey, so this suffices:
        if (o && typeof o === "object") {
            return o;
        }
    }
    catch (e) { }

    return false;
};

function delete_row(e) {
    e.parentNode.parentNode.removeChild(e.parentNode);
    setIdAndName();
    calculateDocumentTottal();
}

function newLine(event, element) {
    if (event.keyCode == 9) {

        if ((productContainer.childElementCount - 1) == getIndex(element.id)) {

            event.preventDefault();
            addProductElement();
        }
    }
}

function getIndex(str) {
    const regex = /\d+/;
    let result = str.match(regex);
    return result[0];
}

function setIdAndName() {

    for (var i = 0; i < productContainer.childElementCount; i++) {

        var items = productContainer.children[i].children;
        items[0].id = `Sales_${i}__ProductId`;
        items[0].name = `Sales[${i}].ProductId`;
        items[1].id = `Sales_${i}__FreeProductId`;
        items[1].name = `Sales[${i}].FreeProductId`;
        items[2].innerHTML = i + 1;
        items[3].id = `Sales_${i}__IsProduct`;
        items[3].name = `Sales[${i}].IsProduct`;
        items[4].id = `Sales_${i}__Name`;
        items[4].name = `Sales[${i}].Name`;
        items[5].id = `Sales_${i}__ProductType`;
        items[5].name = `Sales[${i}].ProductType`;
        items[6].id = `Sales_${i}__Quantity`;
        items[6].name = `Sales[${i}].Quantity`;
        items[7].id = `Sales_${i}__Price`;
        items[7].name = `Sales[${i}].Price`;
        items[8].id = `Sales_${i}__TottalPrice`;
        items[8].name = `Sales[${i}].TottalPrice`;
        items[9].id = `Sales_${i}__VatTypeId`;
        items[9].name = `Sales[${i}].VatTypeId`;
        items[10].id = `Sales_${i}__SaleId`;
        items[10].name = `Sales[${i}].SaleId`;
    }
}

function calculatePrice(event, element) {
    if (event.keyCode === 13 || event.keyCode === 9) {
        element.value = changeComma(element.value);
        let index = getIndex(element.id);
        if (`Sales_${index}__Quantity` == element.id) {
            let priceElement = document.getElementById(`Sales_${index}__Price`);
            let tottalPrice = document.getElementById(`Sales_${index}__TottalPrice`);
            let quantity = Number.parseFloat(element.value);
            let price = Number.parseFloat(priceElement.value);
            if (isNaN(quantity) || element.value == null || element.value == '') {
                quantity = 0;
            }
            if (isNaN(price) || priceElement.value == null || priceElement.value == '') {
                price = 0;
            }

            tottalPrice.value = (quantity * price).toFixed(3);
            tottalPrice.readOnly = true;

        }
        else if (`Sales_${index}__Price` == element.id) {
            let quantityElement = document.getElementById(`Sales_${index}__Quantity`);
            let tottalPrice = document.getElementById(`Sales_${index}__TottalPrice`);
            let quantity = Number.parseFloat(quantityElement.value);
            let price = Number.parseFloat(element.value);
            if (isNaN(quantity) || quantityElement.value == null || quantityElement.value == '') {
                quantity = 0;
            }
            if (isNaN(price) || element.value == null || element.value == '') {
                price = 0;
            }

            tottalPrice.value = (price * quantity).toFixed(3);
            tottalPrice.readOnly = true;

        }
        calculateDocumentTottal(element);
    }
}

function loadProduct(event, element) {
    if (event.keyCode === 13 || event.keyCode === 9) {

        var value = element.value;
        let rowId = getIndex(element.id);
        let isProduct = document.getElementById(`Sales_${getIndex(rowId)}__IsProduct`);

        if (value != null && value != '' && isProduct.checked == true) {

            var route = `/${currentRoute}/Product/GetProductByNameAjax?name=`;

            var loading = document.getElementById('loading');
            loading.classList.remove('invisible');
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let item = JSON.parse(this.responseText);
                    loading.classList.add('invisible');


                    if (item != null && isProduct.checked == true) {
                        isProduct.value = true;
                        if (item.productId != null && item.productId != '') {

                            let productId = document.getElementById(`Sales_${rowId}__ProductId`);
                            productId.value = item.productId;

                            if (item.productType != null && item.productType != '') {
                                let quantityType = document.getElementById(`Sales_${rowId}__ProductType`);
                                quantityType.readOnly = true;
                                quantityType.value = item.productType;

                            }
                            if (item.availableQuantity != null && item.availableQuantity != '') {
                                let quantity = document.getElementById(`Sales_${rowId}__Quantity`);
                                quantity.setAttribute('data-availableQuantity', item.availableQuantity);
                                quantity.setAttribute('placeholder', item.availableQuantity);
                                quantity.value = '';
                            }

                            if (item.price != null && item.price != '') {
                                let price = document.getElementById(`Sales_${rowId}__Price`);
                                price.value = item.price.toFixed(3);
                                price.readOnly = true;
                            }

                            let tottalPrice = document.getElementById(`Sales_${rowId}__TottalPrice`);
                            tottalPrice.value = '';

                            let saleElement = document.getElementById(`Sales_${rowId}__SaleId`);
                            saleElement.value = 0;
                        }
                        setActiveVatType(rowId, item.vatTypeId);


                    }
                    else {
                        let productId = document.getElementById(`Sales_${rowId}__ProductId`);
                        productId.value = 0;

                        let quantityType = document.getElementById(`Sales_${rowId}__ProductType`);
                        quantityType.readOnly = false;

                        let price = document.getElementById(`Sales_${rowId}__Price`);
                        price.readOnly = false;
                        isProduct.checked = false;
                        isProduct.value = false;

                        let saleElement = document.getElementById(`Sales_${rowId}__SaleId`);
                        saleElement.value = 0;
                    }
                }

            };
            xhttp.open("GET", `${route}${value}`, true);
            xhttp.send();
        }
    }
}

function loadProductOnChangeIsProduct(element) {

    var value = element.value;
    let rowId = getIndex(element.id);
    let isProduct = document.getElementById(`Sales_${getIndex(rowId)}__IsProduct`);

    if (value != null && value != '') {

        var route = `/${currentRoute}/Product/GetProductByNameAjax?name=`;

        var loading = document.getElementById('loading');
        loading.classList.remove('invisible');
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let item = JSON.parse(this.responseText);
                loading.classList.add('invisible');


                if (item != null && isProduct.checked == true) {
                    isProduct.value = true;
                    if (item.productId != null && item.productId != '') {

                        let productId = document.getElementById(`Sales_${rowId}__ProductId`);
                        productId.value = item.productId;

                        if (item.productType != null && item.productType != '') {
                            let quantityType = document.getElementById(`Sales_${rowId}__ProductType`);
                            quantityType.readOnly = true;
                            quantityType.value = item.productType;

                        }
                        if (item.availableQuantity != null && item.availableQuantity != '') {
                            let quаntity = document.getElementById(`Sales_${rowId}__Quantity`);
                            quаntity.setAttribute('data-availableQuantity', item.availableQuantity);
                            quаntity.setAttribute('placeholder', item.availableQuantity);
                        }

                        if (item.price != null && item.price != '') {
                            let price = document.getElementById(`Sales_${rowId}__Price`);
                            price.value = item.price.toFixed(3);
                            price.readOnly = true;
                        }
                        setActiveVatType(rowId, item.vatTypeId);

                        let saleElement = document.getElementById(`Sales_${rowId}__SaleId`);
                        saleElement.value = 0;
                    }


                }
                else {
                    let productId = document.getElementById(`Sales_${rowId}__ProductId`);
                    productId.value = 0;

                    let quantityType = document.getElementById(`Sales_${rowId}__ProductType`);
                    quantityType.readOnly = false;

                    let price = document.getElementById(`Sales_${rowId}__Price`);
                    price.readOnly = false;
                    isProduct.checked = false;
                    isProduct.value = false;

                    let saleElement = document.getElementById(`Sales_${rowId}__SaleId`);
                    saleElement.value = 0;
                }
            }

        };
        xhttp.open("GET", `${route}${value}`, true);
        xhttp.send();
    }

}

function changeIsProduct(element) {
    if (element.checked === true) {
        let nameElement = document.getElementById(`Sales_${getIndex(element.id)}__Name`);
        loadProductOnChangeIsProduct(nameElement);
    }
    else {
        let rowId = getIndex(element.id);
        let productId = document.getElementById(`Sales_${rowId}__ProductId`);
        productId.value = 0;

        let quantityType = document.getElementById(`Sales_${rowId}__ProductType`);
        quantityType.readOnly = false;

        let price = document.getElementById(`Sales_${rowId}__Price`);
        price.readOnly = false;

        let isProduct = document.getElementById(`Sales_${getIndex(rowId)}__IsProduct`);
        isProduct.checked = false;
        isProduct.value = false;

        setAllActiveVatType(rowId);
    }
}

function findProductAjax(e) {
    if (e.value.length < 2) {
        return;
    }

    let isProduct = document.getElementById(`Sales_${getIndex(e.id)}__IsProduct`).checked;
    if (isProduct === true) {

        var dataList = document.getElementById('productDataList');
        var value = e.value;
        var route = `/${currentRoute}/Product/FindProductDataListAjax?name=`;
        if (value != '' && !partnerDataListValues.includes(value)) {

            var loading = document.getElementById('loading');
            loading.classList.remove('invisible');
            var xhttp = new XMLHttpRequest();
            xhttp.onreadystatechange = function () {
                if (this.readyState == 4 && this.status == 200) {
                    let result = JSON.parse(this.responseText);
                    dataList.innerHTML = "";
                    loading.classList.add('invisible');
                    partnerDataListValues = [];
                    result.forEach(function (item) {

                        var option = document.createElement('option');
                        option.value = item.name;
                        option.id = item.id;
                        partnerDataListValues.push(item.name);
                        dataList.appendChild(option);

                    });
                }
            };
            xhttp.open("GET", `${route}${value}`, true);
            xhttp.send();
        }
    }


}

function setActiveVatType(row, vatId) {

    let elementVat = document.getElementById(`Sales_${row}__VatTypeId`);
    let optionsVat = elementVat.children;
    for (var i = 0; i < optionsVat.length; i++) {
        if (optionsVat[i].value == vatId) {
            optionsVat[i].selected = true;
            optionsVat[i].disabled = false;
        } else {
            optionsVat[i].disabled = true;
        }
    }
}

function setAllActiveVatType(row) {

    let elementVat = document.getElementById(`Sales_${row}__VatTypeId`);
    let optionsVat = elementVat.children;
    for (var i = 0; i < optionsVat.length; i++) {
        optionsVat[i].disabled = false;
    }
}

function changeComma(value) {
    value = value.replace(',', '.');
    return value;
}

function findPartnerAjax() {
    var dataList = document.getElementById('partnersDataList');
    var value = document.getElementById('findPartner').value;
    var route = `/${currentRoute}/Partner/FindPartnerDataListAjax?name=`;
    if (value != '' && !partnerDataListValues.includes(value)) {

        var loading = document.getElementById('loading');
        loading.classList.remove('invisible');
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let result = JSON.parse(this.responseText);
                dataList.innerHTML = "";
                loading.classList.add('invisible');
                partnerDataListValues = [];
                result.forEach(function (item) {

                    var option = document.createElement('option');
                    option.value = item.name;
                    option.id = item.id;
                    partnerDataListValues.push(item.name);
                    dataList.appendChild(option);

                });
            }
        };
        xhttp.open("GET", `${route}${value}`, true);
        xhttp.send();
    }
}

function loadPartner(event) {
    if (event.keyCode === 13 || event.keyCode === 9) {
        event.preventDefault();
        var value = document.getElementById('findPartner').value;
        var route = `/${currentRoute}/Partner/GetPartnerByNameAjax?name=`;

        var loading = document.getElementById('loading');
        loading.classList.remove('invisible');
        var xhttp = new XMLHttpRequest();
        xhttp.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                let item = JSON.parse(this.responseText);
                loading.classList.add('invisible');
                if (item != null) {
                    document.getElementById('partnerId').value = item.id;
                    document.getElementById('partnerHead').innerText = item.name;
                    document.getElementById('partner').innerText = item.name;
                    document.getElementById('eik').innerText = item.eik;
                    document.getElementById('isVatRegister').innerText = item.isVatRegistered == true ? "ДА" : "НЕ";
                    document.getElementById('vat').innerText = item.vatId;
                    document.getElementById('address').innerText = `${item.country}, ${item.city}, ${item.address}`;
                    document.getElementById('mol').innerText = item.mol;
                    document.getElementById('email').innerText = item.email;

                    if (item.employees != null) {
                        let employeeInputElem = document.getElementById('RecipientEmployee');
                        let recepientEmployeeList = document.getElementById('recipientEmployeeList');
                        recepientEmployeeList.innerHTML = '';
                        for (var i = 0; i < item.employees.length; i++) {
                            let option = document.createElement('option');
                            option.value = item.employees[i].fullName;
                            option.id = item.employees[i].id;
                            recepientEmployeeList.appendChild(option);
                            if (item.employees[i].isActive == true) {
                                employeeInputElem.value = item.employees[i].fullName;
                            }
                        }
                    }

                    document.getElementById('partnerDescription').focus();
                }


            }
        };
        xhttp.open("GET", `${route}${value}`, true);
        xhttp.send();
    }
}

function loadPartnerById(value) {

    var route = `/${currentRoute}/Partner/GetPartnerByIdAjax?id=`;

    var loading = document.getElementById('loading');
    loading.classList.remove('invisible');
    var xhttp = new XMLHttpRequest();
    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {
            let item = JSON.parse(this.responseText);
            loading.classList.add('invisible');
            if (item != null) {
                document.getElementById('partnerId').value = item.id;
                document.getElementById('partnerHead').innerText = item.name;
                document.getElementById('partner').innerText = item.name;
                document.getElementById('eik').innerText = item.eik;
                document.getElementById('isVatRegister').innerText = item.isVatRegistered == true ? "ДА" : "НЕ";
                document.getElementById('vat').innerText = item.vatId;
                document.getElementById('address').innerText = `${item.country}, ${item.city}, ${item.address}`;
                document.getElementById('mol').innerText = item.mol;
                document.getElementById('email').innerText = item.email;

                if (item.employees != null) {
                    let recepientEmployeeList = document.getElementById('recipientEmployeeList');
                    recepientEmployeeList.innerHTML = '';
                    for (var i = 0; i < item.employees.length; i++) {
                        let option = document.createElement('option');
                        option.value = item.employees[i].fullName;
                        option.id = item.employees[i].id;
                        recepientEmployeeList.appendChild(option);
                    }
                }

            }


        }
    };
    xhttp.open("GET", `${route}${value}`, true);
    xhttp.send();
}

function nullCheck(string) {
    if (string == null) {
        return '';
    }
    return string;
}

function preventEnter() {
    $('#productContainer').on('keydown', 'input, select, textarea', function (e) {
        var self = $(this)
            , form = self.parents('form:eq(0)')
            , focusable
            , next
            , prev
            ;

        if (e.shiftKey) {
            if (e.keyCode == 13) {
                focusable = form.find('input,a,select,button,textarea').filter(':visible');
                prev = focusable.eq(focusable.index(this) - 1);

                if (prev.length) {
                    prev.focus();
                } else {
                    form.submit();
                }
            }
        }
        else
            if (e.keyCode == 13) {
                focusable = form.find('input,a,select,button,textarea').filter(':visible');
                next = focusable.eq(focusable.index(this) + 1);
                if (next.length) {
                    next.focus();
                } else {
                    form.submit();
                }
                return false;
            }
    });
}

function validatedate(inputText) {
    const dateformat = /^(0?[1-9]|[12][0-9]|3[01])[\.](0?[1-9]|1[012])[\.]\d{4}$/;
    // Match the date format through regular expression
    let regex = new RegExp(dateformat);

    if (regex.test(inputText)) {
        //Test which seperator is used '/' or '-'
        var opera1 = inputText.split('.');
        lopera1 = opera1.length;

        // Extract the string into month, date and year
        if (lopera1 > 1) {
            var pdate = inputText.split('.');
        }
        else {
            return false;
        }
        var dd = parseInt(pdate[0]);
        var mm = parseInt(pdate[1]);
        var yy = parseInt(pdate[2]);
        // Create list of days of a month [assume there is no leap year by default]
        var ListofDays = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
        if (mm == 1 || mm > 2) {
            if (dd > ListofDays[mm - 1]) {
                return false;
            }
        }
        if (mm == 2) {
            var lyear = false;
            if ((!(yy % 4) && yy % 100) || !(yy % 400)) {
                lyear = true;
            }
            if ((lyear == false) && (dd >= 29)) {
                return false;
            }
            if ((lyear == true) && (dd > 29)) {
                return false;
            }
        }
        return true;
    }
    else {
        return false;
    }
}
