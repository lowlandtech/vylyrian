window.calendarTabStrip = {
    getScrollPosition: (el) => Math.floor(el.scrollLeft),
    getScrollWidth: (el) => Math.floor(el.scrollWidth),
    getClientWidth: (el) => Math.floor(el.clientWidth),
    scrollBy: (el, offset) => el.scrollBy({ left: offset, behavior: 'smooth' }),
    scrollIntoView: (el) => {
        el?.scrollIntoView({ behavior: 'smooth', inline: 'center', block: 'nearest' });
    }
};