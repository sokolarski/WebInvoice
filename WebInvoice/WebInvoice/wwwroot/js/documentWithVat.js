
function calculateDocumentTottal() {
    let cell = document.getElementsByClassName('VatTypeClass');

    let map = new Map();
    for (var i = 0; i < cell.length; i++) {
        let vatId = cell[i].value;
        let tottalPriceElement = document.getElementById(`Sales_${getIndex(cell[i].id)}__TottalPrice`);
        let tottalPrice = parseFloat(tottalPriceElement.value);

        if (map.has(vatId)) {
            map.set(vatId, map.get(vatId) + tottalPrice);
        }
        else {
            map.set(vatId, tottalPrice);
        }
    }

    let vatTypeContainer = document.getElementById('tottalByVatType');

    let vatTypeContainerChild = vatTypeContainer.children;
    if (vatTypeContainerChild != null) {
        for (var i = 0; i < vatTypeContainerChild.length; i++) {
            let index = getIndex(vatTypeContainerChild[i].id);
            if (!map.has(index)) {
                vatTypeContainerChild[i].remove();
                let currentVatType = vatTypes.find(x => x.Id == index);
                if (currentVatType.Percantage == 0) {
                    document.getElementById('FreeText').disabled = true;
                }
            }
        }
    }

    let tottalPriceBasis = 0.0;
    let tottalPriceVat = 0.0;
    let tottalPriceTottal = 0.0;

    for (let [key, value] of map) {
        let vatTypeElement = document.getElementById(`vatType-${key}`);
        if (vatTypeElement == null) {
            let divVatTypeElement = document.createElement('div');
            divVatTypeElement.classList.add('p-2');
            divVatTypeElement.id = `vatType-${key}`;

            let currentVatType = vatTypes.find(x => x.Id == key);

            let vatTypeTittle = document.createElement('p');
            vatTypeTittle.classList.add('p-0', 'm-0', 'text-right', 'font-weight-bold', `vatTypeTittle-${key}`);
            vatTypeTittle.id = `vatTypeTittle-${key}`;
            vatTypeTittle.innerHTML = `ДДС-${currentVatType.Percantage}%`;
            divVatTypeElement.appendChild(vatTypeTittle);

            let vatTypeBasis = document.createElement('p');
            vatTypeBasis.classList.add('p-0', 'm-0', 'text-right', `vatTypeBasis-${key}`);
            vatTypeBasis.id = `vatTypeBasis-${key}`;
            tottalPriceBasis += value;
            vatTypeBasis.innerHTML = value.toFixed(3);
            divVatTypeElement.appendChild(vatTypeBasis);

            let vatTypeVat = document.createElement('p');
            vatTypeVat.classList.add('p-0', 'm-0', 'text-right', `vatTypeVat-${key}`);
            vatTypeVat.id = `vatTypeVat-${key}`;
            let vat = value * (parseFloat(currentVatType.Percantage) / 100);
            tottalPriceVat += vat;
            vatTypeVat.innerHTML = vat.toFixed(3);
            divVatTypeElement.appendChild(vatTypeVat);

            let vatTypeTottal = document.createElement('p');
            vatTypeTottal.classList.add('p-0', 'm-0', 'text-right', `vatTypeTottal-${key}`);
            vatTypeTottal.id = `vatTypeTottal-${key}`;
            let tottal = value + vat;
            tottalPriceTottal += tottal;
            vatTypeTottal.innerHTML = tottal.toFixed(3);
            divVatTypeElement.appendChild(vatTypeTottal);

            vatTypeContainer.appendChild(divVatTypeElement);
            if (currentVatType.Percantage == 0) {
                document.getElementById('FreeText').disabled = false;
            }
        }
        else {

            let currentVatType = vatTypes.find(x => x.Id == key);

            let vatTypeBasis = document.getElementById(`vatTypeBasis-${key}`);
            tottalPriceBasis += value;
            vatTypeBasis.innerHTML = value.toFixed(3);

            let vatTypeVat = document.getElementById(`vatTypeVat-${key}`);
            let vat = value * (parseFloat(currentVatType.Percantage) / 100);
            tottalPriceVat += vat;
            vatTypeVat.innerHTML = vat.toFixed(3);

            let vatTypeTottal = document.getElementById(`vatTypeTottal-${key}`);
            let tottal = value + vat;
            tottalPriceTottal += tottal;
            vatTypeTottal.innerHTML = tottal.toFixed(3);

        }
    }

    document.getElementById('tottalCulumnBasis').innerHTML = tottalPriceBasis.toFixed(3);
    document.getElementById('tottalCulumnVat').innerHTML = tottalPriceVat.toFixed(3);
    document.getElementById('tottalCulumnTottal').innerHTML = tottalPriceTottal.toFixed(3);
}

function addProductElementWithValues(value) {

    let index = document.getElementById('productContainer').childElementCount;

    let product = document.createElement('div');
    product.classList.add('col-12', 'm-0', 'd-flex', 'flex-row', 'productRow');
    product.id = `row${index}`;

    let productId = document.createElement('input');
    productId.setAttribute('type', 'hidden');
    productId.id = `Sales_${index}__ProductId`;
    productId.name = `Sales[${index}].ProductId`;
    productId.classList.add('productId');
    productId.value = value.ProductId;
    product.appendChild(productId);



    let freeProductId = document.createElement('input');
    freeProductId.setAttribute('type', 'hidden');
    freeProductId.id = `Sales_${index}__FreeProductId`;
    freeProductId.name = `Sales[${index}].FreeProductId`;
    freeProductId.classList.add('freeProductId');
    freeProductId.value = value.FreeProductID;
    product.appendChild(freeProductId);



    let productCount = document.createElement('span')
    productCount.classList.add('m-0', 'px-1', 'text-right');
    productCount.style.width = '30px';
    productCount.innerHTML = index + 1;
    productCount.classList.add('productCount');
    product.appendChild(productCount);

    let isProduct = document.createElement('input');
    isProduct.setAttribute('type', 'checkbox');
    isProduct.setAttribute('data-val', 'true');
    isProduct.setAttribute('data-val-required', 'Полето е задължително');
    isProduct.classList.add('m-auto', 'px-1', 'text-right', 'isProduct');
    isProduct.style.width = '25px';
    isProduct.id = `Sales_${index}__IsProduct`;
    isProduct.name = `Sales[${index}].IsProduct`;
    isProduct.checked = value.IsProduct;
    isProduct.value = value.IsProduct;
    isProduct.addEventListener('change', function () { changeIsProduct(this) });
    product.appendChild(isProduct);

    let productName = document.createElement('input');
    productName.classList.add('col-5', 'm-0', 'p-0');
    productName.setAttribute('type', 'text');
    productName.setAttribute('data-val', 'true');
    productName.setAttribute('data-val-required', 'Полето е задължително');
    productName.setAttribute('list', 'productDataList');
    productName.id = `Sales_${index}__Name`;
    productName.name = `Sales[${index}].Name`;
    productName.classList.add('productName');
    productName.addEventListener('input', function () { findProductAjax(this) });
    productName.addEventListener('keydown', function () { loadProduct(event, this) });
    productName.value = value.Name;
    product.appendChild(productName);

    let productType = document.createElement('input');
    productType.classList.add('col', 'm-0', 'p-0');
    productType.setAttribute('type', 'text');
    productType.setAttribute('data-val', 'true');
    productType.setAttribute('data-val-required', 'Полето е задължително');
    productType.id = `Sales_${index}__ProductType`;
    productType.name = `Sales[${index}].ProductType`;
    productType.classList.add('productType');
    productType.value = value.ProductType;
    productType.readOnly = value.IsProduct;
    product.appendChild(productType);


    let productQuantity = document.createElement('input');
    productQuantity.classList.add('col', 'm-0', 'p-0');
    productQuantity.setAttribute('type', 'text');
    productQuantity.setAttribute('data-val', 'true');
    productQuantity.setAttribute('data-val-required', 'Полето е задължително');
    productQuantity.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productQuantity.id = `Sales_${index}__Quantity`;
    productQuantity.name = `Sales[${index}].Quantity`;
    productQuantity.classList.add('productQuantity');
    productQuantity.addEventListener('keydown', function () { calculatePrice(event, this) });
    productQuantity.value = value.Quantity;
    product.appendChild(productQuantity);

    let productPrice = document.createElement('input');
    productPrice.classList.add('col', 'm-0', 'p-0');
    productPrice.setAttribute('type', 'text');
    productPrice.setAttribute('data-val', 'true');
    productPrice.setAttribute('data-val-required', 'Полето е задължително');
    productPrice.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productPrice.id = `Sales_${index}__Price`;
    productPrice.name = `Sales[${index}].Price`;
    productPrice.classList.add('productPrice');
    productPrice.addEventListener('keydown', function () { calculatePrice(event, this) });
    productPrice.value = value.Price.toFixed(3);
    productPrice.readOnly = value.IsProduct;
    product.appendChild(productPrice);

    let productTottal = document.createElement('input');
    productTottal.classList.add('col', 'm-0', 'p-0');
    productTottal.setAttribute('type', 'text');
    productTottal.setAttribute('data-val', 'true');
    productTottal.setAttribute('data-val-required', 'Полето е задължително');
    productTottal.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productTottal.id = `Sales_${index}__TottalPrice`;
    productTottal.name = `Sales[${index}].TottalPrice`;
    productTottal.classList.add('productTottalPrice');
    productTottal.addEventListener('keydown', function () { newLine(event, this) });
    productTottal.value = value.TottalPrice.toFixed(3);
    productTottal.readOnly = true;
    product.appendChild(productTottal);

    let productVatType = document.createElement('select');
    productVatType.classList.add('col', 'm-0', 'p-0', 'VatTypeClass');
    productVatType.id = `Sales_${index}__VatTypeId`;
    productVatType.name = `Sales[${index}].VatTypeId`;
    productVatType.setAttribute('form', 'form1');
    productVatType.setAttribute('data-val', 'true');
    productVatType.setAttribute('data-val-required', 'Полето е задължително');
    productVatType.addEventListener('change', function () { calculateDocumentTottal(this) });
    if (vatTypes) {
        for (var i = 0; i < vatTypes.length; i++) {
            let option = document.createElement('option');
            option.value = vatTypes[i].Id;
            option.innerHTML = `${vatTypes[i].Name}-${vatTypes[i].Percantage}%`;
            if (value.IsProduct) {
                if (value.VatTypeId == vatTypes[i].Id) {
                    option.disabled = false;
                }
                else {
                    option.disabled = true;
                }
            }
            else {
                if (value.VatTypeId == vatTypes[i].Id) {
                    option.selected = true;
                }
            }

            productVatType.add(option);
        }

    }

    product.appendChild(productVatType);

    let saleId = document.createElement('input');
    saleId.setAttribute('type', 'hidden');
    saleId.id = `Sales_${index}__SaleId`;
    saleId.name = `Sales[${index}].SaleId`;
    saleId.classList.add('saleId');
    saleId.value = value.SaleId;
    product.appendChild(saleId);

    let productDelete = document.createElement('span')
    productDelete.classList.add('m-0', 'px-1', 'close');
    productDelete.innerHTML = 'X';
    productDelete.addEventListener('click', function () { delete_row(this) });
    productDelete.classList.add('productDelete');
    product.appendChild(productDelete);




    document.getElementById('productContainer').appendChild(product);
    productName.focus();
    $("#form1")
        .removeData("validator")
        .removeData("unobtrusiveValidation");

    //Parse the form again
    $.validator
        .unobtrusive
        .parse("#form1");
}

function addProductElement() {

    let index = document.getElementById('productContainer').childElementCount;

    let product = document.createElement('div');
    product.classList.add('col-12', 'm-0', 'd-flex', 'flex-row', 'productRow');
    product.id = `row${index}`;

    let productId = document.createElement('input');
    productId.setAttribute('type', 'hidden');
    productId.id = `Sales_${index}__ProductId`;
    productId.name = `Sales[${index}].ProductId`;
    productId.classList.add('productId');
    product.appendChild(productId);

    let freeProductId = document.createElement('input');
    freeProductId.setAttribute('type', 'hidden');
    freeProductId.id = `Sales_${index}__FreeProductId`;
    freeProductId.name = `Sales[${index}].FreeProductId`;
    freeProductId.classList.add('freeProductId');
    product.appendChild(freeProductId);


    let productCount = document.createElement('span')
    productCount.classList.add('m-0', 'px-1', 'text-right');
    productCount.style.width = '30px';
    productCount.innerHTML = productContainer.childElementCount + 1;
    productCount.classList.add('productCount');
    product.appendChild(productCount);

    let isProduct = document.createElement('input');
    isProduct.setAttribute('type', 'checkbox');
    isProduct.setAttribute('data-val', 'true');
    isProduct.setAttribute('data-val-required', 'Полето е задължително');
    isProduct.classList.add('m-auto', 'px-1', 'text-right', 'isProduct');
    isProduct.style.width = '25px';
    isProduct.id = `Sales_${index}__IsProduct`;
    isProduct.name = `Sales[${index}].IsProduct`;
    isProduct.checked = true;
    isProduct.value = true;
    isProduct.addEventListener('change', function () { changeIsProduct(this) });
    product.appendChild(isProduct);

    let productName = document.createElement('input');
    productName.classList.add('col-5', 'm-0', 'p-0');
    productName.setAttribute('type', 'text');
    productName.setAttribute('data-val', 'true');
    productName.setAttribute('data-val-required', 'Полето е задължително');
    productName.setAttribute('list', 'productDataList');
    productName.id = `Sales_${index}__Name`;
    productName.name = `Sales[${index}].Name`;
    productName.classList.add('productName');
    productName.addEventListener('input', function () { findProductAjax(this) });
    productName.addEventListener('keydown', function () { loadProduct(event, this) });
    product.appendChild(productName);

    let productType = document.createElement('input');
    productType.classList.add('col', 'm-0', 'p-0');
    productType.setAttribute('type', 'text');
    productType.setAttribute('data-val', 'true');
    productType.setAttribute('data-val-required', 'Полето е задължително');
    productType.id = `Sales_${index}__ProductType`;
    productType.name = `Sales[${index}].ProductType`;
    productType.classList.add('productType');
    product.appendChild(productType);


    let productQuantity = document.createElement('input');
    productQuantity.classList.add('col', 'm-0', 'p-0');
    productQuantity.setAttribute('type', 'text');
    productQuantity.setAttribute('data-val', 'true');
    productQuantity.setAttribute('data-val-required', 'Полето е задължително');
    productQuantity.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productQuantity.id = `Sales_${index}__Quantity`;
    productQuantity.name = `Sales[${index}].Quantity`;
    productQuantity.classList.add('productQuantity');
    productQuantity.addEventListener('keydown', function () { calculatePrice(event, this) });
    productQuantity.value = 0;
    product.appendChild(productQuantity);

    let productPrice = document.createElement('input');
    productPrice.classList.add('col', 'm-0', 'p-0');
    productPrice.setAttribute('type', 'text');
    productPrice.setAttribute('data-val', 'true');
    productPrice.setAttribute('data-val-required', 'Полето е задължително');
    productPrice.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productPrice.id = `Sales_${index}__Price`;
    productPrice.name = `Sales[${index}].Price`;
    productPrice.classList.add('productPrice');
    productPrice.addEventListener('keydown', function () { calculatePrice(event, this) });
    productPrice.value = 0;
    product.appendChild(productPrice);

    let productTottal = document.createElement('input');
    productTottal.classList.add('col', 'm-0', 'p-0');
    productTottal.setAttribute('type', 'text');
    productTottal.setAttribute('data-val', 'true');
    productTottal.setAttribute('data-val-required', 'Полето е задължително');
    productTottal.setAttribute('data-val-number', 'Стойноста трябва да е число');
    productTottal.id = `Sales_${index}__TottalPrice`;
    productTottal.name = `Sales[${index}].TottalPrice`;
    productTottal.classList.add('productTottalPrice');
    productTottal.addEventListener('keydown', function () { newLine(event, this) });
    productTottal.value = 0;
    product.appendChild(productTottal);

    let productVatType = document.createElement('select');
    productVatType.classList.add('col', 'm-0', 'p-0', 'VatTypeClass');
    productVatType.id = `Sales_${index}__VatTypeId`;
    productVatType.name = `Sales[${index}].VatTypeId`;
    productVatType.setAttribute('form', 'form1');
    productVatType.setAttribute('data-val', 'true');
    productVatType.setAttribute('data-val-required', 'Полето е задължително');
    productVatType.addEventListener('change', function () { calculateDocumentTottal(this) });
    if (vatTypes) {
        for (var i = 0; i < vatTypes.length; i++) {
            let option = document.createElement('option');
            option.value = vatTypes[i].Id;
            option.innerHTML = `${vatTypes[i].Name}-${vatTypes[i].Percantage}%`;
            if (vatTypes[i].IsActive) {
                option.selected = true;
            }
            productVatType.add(option);
        }

    }

    product.appendChild(productVatType);

    let saleId = document.createElement('input');
    saleId.setAttribute('type', 'hidden');
    saleId.id = `Sales_${index}__SaleId`;
    saleId.name = `Sales[${index}].SaleId`;
    saleId.classList.add('saleId');
    saleId.value = 0;
    product.appendChild(saleId);

    let productDelete = document.createElement('span')
    productDelete.classList.add('m-0', 'px-1', 'close');
    productDelete.innerHTML = 'X';
    productDelete.addEventListener('click', function () { delete_row(this) });
    productDelete.classList.add('productDelete');
    product.appendChild(productDelete);



    document.getElementById('productContainer').appendChild(product);
    productName.focus();
    $("#form1")
        .removeData("validator")
        .removeData("unobtrusiveValidation");

    //Parse the form again
    $.validator
        .unobtrusive
        .parse("#form1");
}