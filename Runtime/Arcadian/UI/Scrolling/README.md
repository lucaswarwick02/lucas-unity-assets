# UI.Scrolling

> Click [here](../../../README.md#features) to go back.

## `AutoScrollContent`

> For auto scrolling, this must be added to the content container.

A component that automatically attaches <c>AutoScrollItem</c> components to all <c>Selectable</c> UI elements under its hierarchy, linking them to the nearest parent <c>AbstractAutoScroll</c>. It ensures that new children added at runtime are also registered for automatic scrolling behaviour.

## `HorizontalAutoScroll`

> For auto scrolling, this must be added to the root object.

A Unity component extending AbstractAutoScroll that automatically scrolls horizontally to keep a selected UI element within view. It adjusts the content position based on the selected element’s bounds relative to the viewport and uses smooth coroutine-based motion for transitions.

## `VerticalAutoScroll`

> For auto scrolling, this must be added to the root object.

A Unity component extending AbstractAutoScroll that automatically scrolls vertically to keep a selected UI element within view. It adjusts the content position based on the selected element’s bounds relative to the viewport and uses smooth coroutine-based motion for transitions.