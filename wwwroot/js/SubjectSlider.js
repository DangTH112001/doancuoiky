$(document).ready(function () {
  $('.subject-slider').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    autoplay: true,
    autoplaySpeed: 1000,
    arrow: true,
    dots: true,
    centerMode: true,
    prevArrow:
      `<button type='button' class='slick-prev slick-arrow'><i class='bx bx-left-arrow-alt'></i></button>`,
    nextArrow:
      `<button type='button' class='slick-next slick-arrow'><i class='bx bx-right-arrow-alt'></i></button>`
  });
});