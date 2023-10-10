# [Changelog](http://keepachangelog.com/en/1.0.0/):

## [Unreleased]

### Added

- Now, the toolbar will automatically show up when you enter tablet mode, and hide when you enter desktop mode. This can be disabled in the settings.
- Added `dock` and `undock` actions that allow to perform toolbar docking with a button instead of a context menu.
- Added `docked_right`, `docked_left` and `docked_top` options to `visibility` property. Now, buttons can be shown only in some docking modes.

### Changed

- `layout` action now only needs layout name to be specified instead of its full path (for example, `action layout foxe` instead of `action layout files/layouts/foxe.yaml`). Paths will still work as long as the layout specified is in the `files/layouts` directory.
- Layout properties `external_theme` and `theme` no longer work. Now, you must select a theme in the context menu independently from a layout. See "Migrating to 2.0" section for the full guide.
- Theme properties `button_size`, `margin`, `min_opacity`, `max_opacity` have been moved directly to layout yaml. Themes can no longer influence these properties. See "Migrating to 2.0" section for the full guide.
- Now the app will start in hidden mode.
- Updated to .NET7
- Optimized layout changing.

### Fixed

- Context menu color will now match theme color.
- Quit button doesn't hang the app anymore.

## [v1.2.0] - 24.10.2021

### Added

- Added `hide` action which hides the toolbar.

### Fixed

- Autostart now works properly.

<hr/>

## [v1.1.0] - 06.10.2021

### Added

- Added the titlebar with the ability to minimize the window.
- Added `visibility` attribute - now, some buttons can be present in either docked or undocked mode.
- When updating, the installer will make a backup.
- Toggle buttons now have visible state.
- Added `repeat` action. Repeating buttons will continue triggering key presses if the button is being held down.
- Added update checker.
- Added a layout for Leonardo.

### Changed

- Expanded default layouts.

### Fixed

- Editing a layout no longer throws false errors.
- Installer now copies only its own files.

<hr/>

## [v1.0.0] - 26.09.2021

helo :D
