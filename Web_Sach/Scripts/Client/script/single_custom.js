
$(document).ready(function()
{
	"use strict";
	initStarRating();
	function initStarRating()
	{
		if($('.user_star_rating li').length)
		{
			var stars = $('.user_star_rating li');

			stars.each(function()
			{
				var star = $(this);
				var dem = 0;
				star.on('click', function()
				{
					var i = star.index();
					dem = 0;
					stars.find('i').each(function()
					{
						
						$(this).removeClass('fa-solid');
						$(this).addClass('fa-regular');
					});
					for(var x = 0; x <= i; x++)
					{
						$(stars[x]).find('i').removeClass('fa-regular');
						$(stars[x]).find('i').addClass('fa-solid');
						dem++;
					};
					$('#txtRate').val(dem);
					console.log(dem);
				});
			});
		}
	}
});