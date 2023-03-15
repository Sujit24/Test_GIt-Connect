define([
// Load the original jQuery source file
  "https://maps.googleapis.com/maps/api/js?sensor=true"
], function () {
    // Tell Require.js that this module returns a reference to jQuery
    return window.google;
});