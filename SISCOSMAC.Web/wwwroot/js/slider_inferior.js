$(document).ready(function(){
  $('.slider_inferior').slick({
  infinite: true,
  slidesToShow: 5,
  slidesToScroll: 1,
	  centerMode: true,
  		centerPadding: '0px',
  responsive: [
    {
      breakpoint: 1024,
      settings: {
        slidesToShow: 5,
        slidesToScroll: 1,
      }
    },
    {
      breakpoint: 600,
      settings: {
        slidesToShow: 5,
        slidesToScroll: 1
      }
    },
    {
      breakpoint: 480,
      settings: {
        slidesToShow: 3,
        slidesToScroll: 1
      }
    }
  ]
	  
});
});
		