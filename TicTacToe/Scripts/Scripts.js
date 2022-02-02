//set active nav item
function setActiveNavItem(navItem) {

    $('.nav-link').removeClass('active');

    if ($('.' + navItem)) {

        $('.' + navItem).addClass('active');
    }
}