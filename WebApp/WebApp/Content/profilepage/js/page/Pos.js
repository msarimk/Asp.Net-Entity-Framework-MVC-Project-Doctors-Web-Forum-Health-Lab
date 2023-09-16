let show_details = document.querySelector('.btn-cusDetails');
let popushow = document.querySelector('.popup-customer-details');
let popupadded = document.querySelector('.add-popdetails');

show_details.addEventListener('click', () => {
    popushow.classList.toggle('show');

});
popupadded.addEventListener('click', () => {
    popushow.classList.remove('show');
});






let btn_splits = document.querySelector('#split');
let split_popup = document.querySelector('.split_popup');

btn_splits.addEventListener('click', () => {
    split_popup.classList.toggle('show');

});


