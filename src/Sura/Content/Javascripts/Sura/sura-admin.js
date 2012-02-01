/*! http://mths.be/placeholder v1.8.7 by @mathias */
(function(o,m,r){var t="placeholder" in m.createElement("input"),q="placeholder" in m.createElement("textarea"),l=r.fn,k;if(t&&q){k=l.placeholder=function(){return this};k.input=k.textarea=true}else{k=l.placeholder=function(){return this.filter((t?"textarea":":input")+"[placeholder]").not(".placeholder").bind("focus.placeholder",s).bind("blur.placeholder",p).trigger("blur.placeholder").end()};k.input=t;k.textarea=q;r(function(){r(m).delegate("form","submit.placeholder",function(){var a=r(".placeholder",this).each(s);setTimeout(function(){a.each(p)},10)})});r(o).bind("unload.placeholder",function(){r(".placeholder").val("")})}function n(b){var c={},a=/^jQuery\d+$/;r.each(b.attributes,function(d,e){if(e.specified&&!a.test(e.name)){c[e.name]=e.value}});return c}function s(){var a=r(this);if(a.val()===a.attr("placeholder")&&a.hasClass("placeholder")){if(a.data("placeholder-password")){a.hide().next().show().focus().attr("id",a.removeAttr("id").data("placeholder-id"))}else{a.val("").removeClass("placeholder")}}}function p(){var d,e=r(this),c=e,a=this.id;if(e.val()===""){if(e.is(":password")){if(!e.data("placeholder-textinput")){try{d=e.clone().attr({type:"text"})}catch(b){d=r("<input>").attr(r.extend(n(this),{type:"text"}))}d.removeAttr("name").data("placeholder-password",true).data("placeholder-id",a).bind("focus.placeholder",s);e.data("placeholder-textinput",d).data("placeholder-id",a).before(d)}e=e.removeAttr("id").hide().prev().attr("id",a).show()}e.addClass("placeholder").val(e.attr("placeholder"))}else{e.removeClass("placeholder")}}}(this,document,jQuery));

/* Foundation v2.1.5 http://foundation.zurb.com */
$(document).ready(function () {

    var page = window.location.pathname.split('admin/')[1];
    $('li.nav-' + page).addClass('current-page');
    
	/* Use this js doc for all application specific JS */

	/* TABS --------------------------------- */
	/* Remove if you don't need :) */

	function activateTab($tab) {
		var $activeTab = $tab.closest('dl').find('dd.active'),
				contentLocation = $tab.attr("href") + 'Tab';

		//Make Tab Active
		$activeTab.removeClass('active');
		$tab.parent().addClass('active');

    	//Show Tab Content
		$(contentLocation).closest('.tabs-content').children('li').hide();
		$(contentLocation).show();
	}

	$('dl.tabbed').each(function () {
		//Get all tabs
		var tabs = $(this).children('dd').children('a');
		tabs.click(function (e) {
			activateTab($(this));
		});
	});

	if (window.location.hash) {
		activateTab($('a[href="' + window.location.hash + '"]'));
	}
   
	/* ALERT BOXES ------------ */
	$(".alert-box").delegate("a.close", "click", function(event) {
    event.preventDefault();
	  $(this).closest(".alert-box").fadeOut(function(event){
	    $(this).remove();
	  });
	});


	/* PLACEHOLDER FOR FORMS ------------- */
	/* Remove this and jquery.placeholder.min.js if you don't need :) */

	$('input, textarea').placeholder();



	/* UNCOMMENT THE LINE YOU WANT BELOW IF YOU WANT IE6/7/8 SUPPORT AND ARE USING .block-grids */
//	$('.block-grid.two-up>li:nth-child(2n+1)').css({clear: 'left'});
//	$('.block-grid.three-up>li:nth-child(3n+1)').css({clear: 'left'});
//	$('.block-grid.four-up>li:nth-child(4n+1)').css({clear: 'left'});
//	$('.block-grid.five-up>li:nth-child(5n+1)').css({clear: 'left'});



	/* DROPDOWN NAV ------------- */

	var lockNavBar = false;
	$('.nav-bar a.flyout-toggle').live('click', function(e) {
		e.preventDefault();
		var flyout = $(this).siblings('.flyout');
		if (lockNavBar === false) {
			$('.nav-bar .flyout').not(flyout).slideUp(500);
			flyout.slideToggle(500, function(){
				lockNavBar = false;
			});
		}
		lockNavBar = true;
	});
  if (Modernizr.touch) {
    $('.nav-bar>li.has-flyout>a.main').css({
      'padding-right' : '75px',
    });
    $('.nav-bar>li.has-flyout>a.flyout-toggle').css({
      'border-left' : '1px dashed #eee'
    });
  } else {
    $('.nav-bar>li.has-flyout').hover(function() {
      $(this).children('.flyout').show();
    }, function() {
      $(this).children('.flyout').hide();
    })
  }


	/* DISABLED BUTTONS ------------- */
	/* Gives elements with a class of 'disabled' a return: false; */
  

});

function redirect(url) {
    window.location.href = url;
}