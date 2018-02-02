# R7.University Changelog

## Version 2.0.0

This is a major release dedicated to the transition to new obrnadzor.gov.ru recommendations.

- Updated information structure and obrnadzor.gov.ru microdata markup across all affected modules to match new recommendations.
- New models: Year, EduVolume, Contingent.
- Convert EduProgramProfileForm model to the EduProgramProfileFormYear model.
- New modules: ScienceDirectory, EduVolumeDirectory (2 modes), ContingentDirectory (4 modes).
- DivisionDirectory module: New mode to display governing divisions.
- Add Created/LastModified fields to documents (#220).
- Mark edu. programs adopted for peoples with disabilities and implemented using E-learning and distance education technologies (#190).
- Add IsGoverning flag to the Divison model, convert IsVirtual flag to IsSingleEntity.
- Do not display division title/link for occupied positions in single-entity divisions.  
- EduProgramProfileDirectory module: Practices, admission and contingent movement column are visible for editors only, will be removed later.
- EduProgram module: Support for displaying edu. forms available for admission.
- UI, Security: Extract EditEduProgramProfileDocuments control (#250).
- UX: Close Bootstrap modal popup by ESC key, add vertical scrollbars when content is too large. 
- Bugfixes: EditAchievements: UrlControl state persists even after loading new item (#231).
- Performance: DivisionDirectory: Greatly improve performance by reducing database calls to just one per request. 
- Configuration: Allow to configure vocabulary names for organization structure and stored working hours (#212).

### Installation notes

Due to project structure changes, following module folders inside the `DesktopModules/MVC/R7.University/` folder are no longer needed
and you should remove them after install: `Employee`, `EmployeeList`, `EmployeeDirectory`, `Division`, `DivisionDirectory`,
`EduProgram`, `EduProgramDirectory`, `EduProgramProfileDirectory`.

## Version 1.15.1

### Most notable changes:

- UX: More compact display option for EduProgramProfileDirectory Documents mode using popups and pre-rendered tables (#176).
- Integration, Productivity: Implemented autosync between divisions and taxonomy (#164).
- Model: Divisions and edu. program (profile) association reimplemented as many-to-many (#163).
- Model: Separated time to learn hours and years/months for edu. program profile forms (#182).
- UI: Employee views and all jQuery popups converted to Bootstrap (#165, #194).
- Integration: Fixed module name in search results/settings cannot be customized via SearchableModules.resx (#167).
- UX, Prodictivity: Added rollback for delete command and colorful state markers for Grid&Form (#178).
- Performance, Reliability: More faster and reliable update algorithm for Grid&Form (#178).
- Performance: Disabled viewstate for most main views (#207).
- Performance: Implemented data cache for EmployeeDetails, DivisionDirectory and EduProgramDirectory modules (#208).
- Install location changed to ~/DesktopModules/MVC/R7.University.
- Portal-level R7.University.yml file is no longer required. You can create one to override default settings.

### Other changes:

- Division: Main view converted to Razor.
- Performance: Default cache time increased to 3600.
- Proposed fix for nasty DnnUrlControl behavior when it broke after async postback.
- UI: Division: Address displayed as a separate field, corresponding module setting removed (#191).
- UI: Added "Audit" tab for edit views for main entities (#198).
- UI: EditEmployee: Added Contacts tab (#197).
- UI: EmployeeList: Add label for occupied positions to help distinct similar achievenment and position names (#209).
- UX: Added validation to ensure edu. program profile forms is unique (#195).
- UX: Added validation to edu. form time to learn (#193).
- UX: Make more fields required: division's title, edu. program profile languages.
- Editing of all related entities implemented via reusable Grid&Form-based controls (#196).
- Edit documents: Some work on determining folder more reliable (#173).
- Many code refactorings considering CQRS, moving code to R7.Dnn.Extensions and more.

### Installation notes:

Due to newfound DNN packaging system limitation, installation of v1.15 version 
(or upgrade from any lower version) must be done in three steps:

1. Install v1.15.0 package first.
2. Then run 01.15.00.PostInstall.SqlDataProvider script from Host/SQL.
3. After that, install v1.15.1 package.

## Version 1.14.1

This release focused on resolving issues with editing and displaying employee achievements, introdiced in v1.14.0.

- Fix duplicate AchievementType records created then editing employee achievements (#170).
- Resolve errors on editing and deleting employee achievements.
- Fix custom achievement type name not displayed.
- Make employee achievement title form field required.
- Improve performance of EmployeeDirectory (teachers mode) and EduProgramProfileDirectory (documents mode) by flattening viewmodels (#130).
- More code refactoring.

## Version 1.14.0

- Only admins can delete employees, divisions, edu. programs and edu. program profiles (#22).
- Module settings splitted into two collapsible panels: "General Settings" and Display Settings".
- Only admins can change module-level (general) settings (#157).
- Implemented validation of filenames for documents based on FilenameFormat regex (#162).
- Added AchievementTypes table for system and custom achievement types (#113).
- Four new system achievement types: title, authorship, professional training and professional retraining.
- Enabled partial rendering for all edit forms which require postback.
- Launchpad: enabled partial rendering for main view.
- Launchpad: module actions displayed only for tables configured in settings.
- Employee details popup will close by clicking (x) without page reload.
- Selected file preserved between employee achievement editing postbacks.
- Major code refactoring.

## Version 1.13.0

This release is focused on transition to DNN 8.

- The minimum DNN version is v8.0.4 now.
- DnnListBox'es and single remaining DnnComboBox replaced with generic ASP.NET controls.
- Updated R7.DotNetNuke.Extensions dependency to v0.10.0.
- Updated R7.ImageHandler dependency to v1.1.0.
- Switch to use SettingsRepository to work with settings.
- Fixed achievement suffix not displayed in the employee details (#160).
- Workaround for employee details popups issue in IE if page URL contains Unicode characters (#159).
- Speedup adding new documents by autoselecting first document's folder (#161).
- Fixed minor issues on clean install.
- Some code moved to the R7.DotNetNuke.Extensions library.

## Version 1.12.1

This is a mostly polishing release featuring localization update and some after-thought improvements for the DivisionDirectory module.

- Provide more semantics on divisions without head employee in the DivisionDirectory module.
- Hide informal divisions only from regular users, also grey out informal divisions for editors in the DivisionDirectory module.
- Fixed DivisionDirectory module in the Search mode produce wrong HTML markup (#156).
- Fixed (again) incorrect check for duplicate disciplines (#119).
- Finished russian UI translation (some UI strings still unstanslatable).
- Some localization fixes for Launchpad and Division modules.
- ControlTitle resource keys updated.
- Fixed bg color not applied to table rows on hover for not published items.
- Build system improvements, including build support for Visual Studio Code (mostly pushed from R7.Epsilon).

## Version 1.12.0

This release features GNU AGPLv3+ transition as well as some division model changes, usability improvements and fixes.

- R7.University code is now under the terms of GNU Affero GPL version 3 or any later version (#140). AGPL signature was added to major views.
- Fixed bug with edu. form learn time could be entered in hours only (#150).
- Fixed encoding issue with employee achievement description and title in the employee details view.
- Division now have Address property for post address, used along with existing Location property (#133).
- Division now have IsInformal property to mark informal divisions, which can be filtered out from DivisionDirectory view (#145).
- All division selection now handled via new DivisionSelector control (#142).
- Division module now have a setting to control how address and location is displayed.
- EduProgramDirectory will no longer crash on unknown language, but will display "<unknown language>" instead.
- Dropped support for setting CSS class for division's document according to URL type and file extension (not used).
- More target="_blank" attribute for links (#134).
- Division module now hides itself for regular users if division is not published - same as the Employee and EduProgram modules (#132).
- Restored sorting in edu. program profile dropdown in employee disciplines editing.
- Improved employee disciplines editing by preserving disciplines field content between operations and by smarter switching between edu. levels (#149).
- Edu. program edit form now have separate tab with list of profiles and add/edit buttons (#139).
- Disciplines linked to not published edu. program profiles in the employee edit form now marked by color .
- Not published profiles in the edu. program edit form now marked by color.
- Not published documents in the edit form now marked by color.
- Disciplines with not published edu. program profiles now removed from employee details view (#153).
- Implemented ability to add default profile along with new edu. program (#151).
- Cancel now close popup window w/o reloading the page (#152).
- Added text labels to employee details view to describe positions and constacts info sections.
- General localization resources update, more UI parts translated to russian.
- Some autocomplete combobox and CSS improvements.
- Code refactoring and cleanup.

## Version 1.11.0

- Implemented ability to store and display time to learn in hours (#129).
- [EduProgramProfileDirectory] Added module setting to allow content filtering by division (#137).
- [EduProgramProfileDirectory] Added russian translation for module settings.
- [Employee modules] Restored occupied positions grouping and sorting, fixed position suffix display (#138).
- [Employee modules] Added "Education Level" columns to the disciplines grids in EditEmployee and ViewEmployeeDetails forms (#136).
- [Employee modules] Restore grid headers localization in ViewEmployeeDetails form.
- [Employee modules] Make grids in EditEmployee form take full width.
- [EmployeeDirectory] Display education level title along with edu. profile title in the headers in "Teachers" view mode (#136).
- Some code refactoring and cleanup.

## Version 1.10.1

This release focuses to switching from DAL2 to Entity Framework as data access layer.

- Improve performance of many modules by using less database queries per module load.
- Remove University_GetHeadEmployee DB stored procedure.
- Remove vw_University_EmployeeDisciplines DB view.
- Replace ItemID field in University_Documents table by two numeric foreign keys.
- Implement more reliable cache handling.
- [EduProgram] Fix autotitle setting label missing help.
- Upgrade YamlDotNet to 3.9.0.

## Version 1.9.1

- [EduProgram] Group "accredited-to" fields in UI.
- [EduProgram] Make profile display more clean (don't use UL, use H3 header for title).
- [EduProgram] Add autotitle tab-specific setting.
- [EduProgram] Sort edu. program profiles.
- [EduProgramDirectory] Expand title column.
- [Employee] [EmployeeDetails] Make autotitle setting tab-specific.

## Version 1.9.0

### End-user changes:

- New EduProgram module which displays basic info about edu. program and its profiles.
- Fix #123: [EmployeeDirectory] Education and Trainig columns show same content.
- Fix #120: dnn-ac-combobox rendered incorrectly in Chrome.
- Implement #121: Add indexes to FKs and other fields used in queries (improve performance).
- Implement #122: Make barcode display optional for an employee.
- Implement #126: [EditDocuments] Sort documents on load.
- Package friendly names for modules now in R7.University.ModuleName format.
- [Launchpad] Fix action label generation.
- [Model] Ability to define education sublevels for edu. profiles.
- [Model] Add homepage property for edu. program.
- [Model] Add division relation for edu. program and edu. program profile.
- [EduProgramDirectory] display edu. program title as link to homepage.
- [EduProgramDirectory] could display edu. programs for single division (specified in settings).
- [EduProgramDirectory] displays edu. programs for all available edu. levels, if no edu. levels was set in settings.
- [UX] Improve selection of edu. programs and edu. program profiles in edit and settings forms.
- [UX] Use Icon API to get module action icons, use different icons for different action types.
- [UX] Enable AJAX for entire EditEmployee form.
- License changed to GNU GPL v3 or any later.
- Russian translation update.

### Developer changes:

- dnn-ac-combobox now preserve autopostback behavior of underlying dropdown.
- Extract viewmodelbase classes, major DAL code refactoring.
- Add xUnit-based tests project (include config test, basic document model extensions test).

## Version 1.8.1

This release provide fixes for bugs, found in 1.8.0.

- [Launchpad] Fix exception: edu. program doesn't have AccreditedToDate property anymore.
- [Launchpad] Fix binding error in old EmployeesTable code.
- [EditEmployee] Fix #119: Incorrect check for duplicate disciplines.
- Fix uninstall script.

## Version 1.8.0

### Major end-user changes:

- Fix #102: [EditEmployee] Validate duplicate discipline entries.
- Fix #103: [Employee] Cannot change module permissions.
- Fix #108: [Employee Directory] Greatly improve performance in the teachers view mode by reducing number of db requests and use caching.
- Implement #41: Global settings in YAML format (see R7.University.yml file in the portal root).
- Implement #49: [EmployeeList] New module setting to hide head employee from the list.
- Implement #84: [EmployeeDirectory, DivisionDirectory] Less restrictive search.
- Implement #96: [A11y] Use accessible header for all public grids.
- Implement #98: [Model] Document type now required for a document.
- Implement #100: [Model] use StartDate/EndDate instead of IsPublished for an employee.
- Implement #104: [EditEmployee] Improve selection of edu. program profiles by adding edu. level dropdown.
- Implement #112: [EduProgramDirectory] Add support for new system document type: professional standard.
- Implement #115: [EduProgramProfileDirectory] now can display multiple document links of any doc. type in a single cell using new Group property.
- [Model] Now you cannot delete edu. level, document type or edu. program entry if there some linked entries exists.
- [Employee, EmployeeList] Add validators for PhotoWidth fields.
- [Employee Directory] Hide not published edu. profiles from regular users.
- [EduProgramProfileDirectory] Improve performance by caching.
- [EduProgramDirectory] Module now can display multiple standard links in a single cell.
- [EduProgramDirectory] Add settings to show or hide edu. level, prof. standard and generation columns.
- [UI] Not published items preserve distinct look on mouse hover.

### Notable developer changes:

- The DotNetNuke.R7 library replaced with R7.DotNetNuke.Extensions.
- Add formal R7.ImageHandler dependency in the manifest.
- Implement #106: Replace AjaxControlTookit.Combobox with jQuery UI based script, drop AjaxControlToolkit dependency.
- [EmployeeList, Employee] Drop render cache implementation in favor of data cache.
- [EmployeeList, Employee] Drop render cache implementation in favor of data cache.
- Massive refactoring and cleanup, close many code tasks (many new also added), apply new formatting rules.

## Version 1.7.5

This release provide some fixes for EditEmployee form.

- Fix #101: EditEmployee: DnnUrlControl wrong behavior.
- EditEmployee: Fix "Achivements" typo in resources.
- EditEmployee: Add missing colons for labels in russian tranlation.

## Version 1.7.4

This release brings extra usability improvements.

- Fix: Apply proper obrnadzor.gov.ru microdata tags in EduProgramProfileDirectory.
- EmployeeDirectory: Allow filter teachers by edu. levels specified in module settings.
- EmployeeDirectory: Display start year for training and education.
- EditEmployee, EditDivision, EditAchievement: Replace DnnComboBox with DropDownList.
- EditDivision: Split division edit form onto 4 tabs.
- EditDocuments: Add advanced properties panel, reorder fields.
- EditEmployee: Add advanced properties panel, reorder fields.
- EditEmployee: Use AjaxControlToolkit.ComboBox to select achievements, switch fields visibility using AJAX.
- EditEmployee, EditEduForms: Group related controls on one line (experiment).
- Replace all instances of old UrlControl with new DnnUrlControl.
- Adjust AjaxControlToolkit.ComboBox button width.
- Some code refactoring and cleanup.

## Version 1.7.3

This release brings some usability improvements.

- Add validation to some numeric fields.
- Add russian translation to EditEduLevel and EditPosition forms.
- EditEduForms: Replace edu. forms dropdown with radiobuttons.
- EditEduForms: More clear labels for add/update buttons.
- EditEduForms: Better formatting of time to learn values.
- EditEduProgramProfile: Add button to edit selected edu. program.
- EditDocuments: Don't reset document and URL types.

## Version 1.7.2

- Launchpad: Quickfix for null ShortTitle (relate to #32).
- Update DotNetNuke.R7 to 0.3.1.
- Fix #99: Issues when creating edu. program profiles.

## Version 1.7.1

- Upgrade AjaxControlToolkit to 15.1.4.
- Update DotNetNuke.R7 to 0.3.0.
- Specify DotNetNuke.R7 as managedPackage dependency, don't ship DotNetNuke.R7.dll assembly itself.
- Fix Coverity defect: CID 48451.
- Launchpad: fix russian translation of tabs.

## Version 1.7.0

This release focuses on adding new EduProgramProfileDirectory module.

- New EduProgramProfileDirectory module with two display modes.
- Added accreditation dates and languages to educational program profile model.
- Removed education type from education level model.
- EditEduProgramProfile: Implemented editing educational program profile documents and forms.
- Launchpad: Tabs now localizeable, russian translation added.
- Launchpad: Search results now preserved until cleared.
- Launchpad: Improved performance by binding only required tab.
- Launchpad: Use single button to add new items.
- Launchpad: Disabled editing support for documents table (no edit control).
- Update and fix unistall script.
- (dev) Massive code refactoring by using model interfaces and control base classes (more to be done).
- (dev) Travis CI integration.

## Version 1.6.3

### End-user changes:

- Display employee and division barcodes in jQuery UI popups.
- EmployeeDetails: Remove barcode tab.
- EmployeeDetails: Use hyperlink as available description indicator for achiements.
- Division: Remove Division_BarcodeWidth setting.
- Fix #87: Division module shows not published subdivisions.
- Add label for subdivisions list, hide subdivisions panel if there are no items to display.

### Developer changes:

- Implement document editing as user control.
- Allow use viewmodel context in custom controls.
- Include .pdb's in local deploy to allow debugging (.NET only).

## Version 1.6.2

- EditEmployee: Better sorting for edu. program profiles dropdown.
- EmployeeDetails: Show achivement description in the jQuery UI popup.
- EmployeeDetails: Don't set tooltip for header row.
- EmployeeDetails: Hide disciplines tab if there are no records.
- EmployeeDetails: Use default popup window height.
- EmployeeList: Show employee name as link to details popup.
- Fix treeview width issues in admin UI parts.

## Version 1.6.1

- DivisionDirectory, EmployeeDirectory: Use Bootstrap styles for grids in search mode.
- DivisionDirectory, EmployeeDirectory: Hide not published divisions in search treeviews.
- DivisionDirectory: Hide not published divisions from regular users, mark them for editors.
- MSBuild.Community.Tasks dependency resolved via NuGet.
- Changes to allow build project using xbuild on Mono 4.
- Include module controls and resources in local deploy (project source files now relocatable).

## Version 1.6.0

- Add EduProgramDirectory module.
- Add sort index to the educational level model.
- Educational level and document type models now cacheable.

## Version 1.5.1

- Fix null reference exception then accessing empty viewstate.

## Version 1.5.0

Main goal: Enhance DivisionDirectory module obrnadzor.gov.ru mode.

- Add ability loose-link divisions and head employees by specifying required position.
- Partial support for virtual divisions (which generally w/o employees).
- EditDivision: support for new division properties.
- Implement hierarchical sorting of divisions.
- Some visual enhancements - indents, block dividers.
- Other small updates and fixes.

## Version 1.4.0

Main goal: Add support for documents and document types and allow to manage educational program documents.

## Version 1.3.1

- Fix #77: EditEmployee: crash when opening in DNN 7.4.2.
- Update uninstall script to drop tables and views added in 1.3.0.
- Set library and module CoreVersion dependency to DNN 7.4.0.

## Version 1.3.0

**Important change:** Educational programs and educational profiles were separated.

Install script perform DB schema change and also safely move data between new tables.

**Before upgrade:** Make sure, what University_EduProgram table don't have any records
with null or empty Code field and all Code values strictly relate to certain educational program.
Also make database backup before upgrading!

- Add audit and StartDate/EndDate properties to educational program.
- Removed obsolete employee properties: NamePrefix, Disciplines, AcademicDegree, AcademicTitle and corresponding UI/code parts.
- Workaround for #74: External links in achievements not working.
- AjaxControlToolkit.ComboBox upgraded to 15.3.1.
- Code and DB schema refactoring.

## Version 1.2.1

- Support for educational program profiles.
- Implement #46: Launchpad: Added text search for all tables.
- EditEmployee: Use AjaxControlToolkit.ComboBox for educational program dropdown.

## Version 1.2.0

- Support for employee and educational programs / disciplines binding.
- EditEmployee: AcademicTitle & AcademicDegree field now hidden.
- AcademicTitle & AcademicDegree implemented as separate achievement types.
- EmployeeDirectory: New display mode to show all teachers groupped by educational programs (obrnadzor.gov.ru requirements).
- DivisionDirectory: New display mode to show all divisions (obrnadzor.gov.ru requirements).

## Version 1.1.8

- Implemented #63: Added EmployeeDetails module.
- EmployeeDetails: Optimize & fix table painting script.
- Localization updated.
- Other small improvements and fixes.

## Version 1.1.7

- Fixed #65: DivisionDirectory shows unpublished head employees.
- Implemented / fixed #64: Employee: Cross-portal Tab URLs.
- EditEmployee: Fixes and some refactoring of the UI.
- EmployeeList: Fix wrong edit control invoked.
- Employee, EmployeeList, Division: Added module URL to the search.
- Use separate CSS files for view and edit/settings controls.
- Packaging project renamed to avoid conflicts with NuGet.

## Version 1.1.6 RC

- New DivisionDirectory module.
- EmployeeDirectory, DivisionDirectory: Added edit links to the search results.
- Make sure that shared employee / division controls have same key names in different modules.
- Other small improvements and fixes.

## Version 1.1.5

- EditEmployee: Control now fully localizable.
- EditEmployee: Added russian localization resources.
- EditEmployee: Display document URL in achievements edit form.
- Implemented #55: ViewEmployeeDetails: Added edit button.
- Implemented #56: EmployeeDirectory: Mark unpublished employees.
- Implemented #58: ViewEmployeeDetails: Paint tables with dnnGrid classes.
- Implemented #32: Now short titles also optional for division and position entities.
- Fixed #59: EmpoyeeDirectory: Missing division link.
- EmployeeList: DB requests reduced from (1 + 2*NEmployees) to 3.
- EditDivision: Use treeview to select parent division.
- Other small improvements and fixes.

## Version 1.1.4

- Fixed #52: Pressing Enter in the search entry should activate search.
- Fixed #54: EmployeeDirectory: Could search with empty search phrase including subdivisions.
- Implemented #51: EmployeeDirectory: Search by position titles.
- Other small improvements and fixes.

## Version 1.1.3

- New EmployeeDirectory module provides search for employees.
- ViewEmployeeDetails: fixes for non-popup mode.
- Other small improvements and fixes.

## Version 1.1.2

- Ability to lookup for photo by employee name (also supports transliteration for cyrillic (russian) names).
- Implemented #39: Position entity: Added IsTeacher field and corresponding UI.
- Implemented #45: Employee entity: Added Disciplines field and corresponding UI.
- Implemented #47: Employee entity: Default employee photo images.
- Partially implemented #46: Launchpad: Search / filter functionality by employees.
- Other small improvements and fixes.

## Version 1.1.1

- EmployeeList module: Better employee sorting by adding some weight to the prime positions in the current division (applies to sorting by max. weight only).
- Employee module: Shows edit / details actions only when employee exists, not just defined in module settings.
- Employee entity: OtherName now optional.
- EditEmployee control: Employee name fields now properly validated.
- EditEmployee control: Cancel buttons in position and achievement edit forms always visible now.
- Implemented #10: Employee, Division entities: Added WebSiteLabel field to make website links more readable.
- Implemented #36: Employee module: Ability to display employee info by userid querystring param on the profile page.
- Implemented #38: Division entity: Added DocumentURL field for the division's main (regulations) document.
- Implemented #44: EditEmployee control: Clear achievement form when editing is done.
- Fixed #42: EditEmployee control: Document URL doesn't always set properly when editing employee achievements.
- Fixed #43: EditEmployee control: Missing currently selected tab on postback.
- Other small improvements and fixes.

## Version 1.1.0

- Added DB shema, edit and view UI for employee achievements, including common achievement types.
- As a result, AcademicTitle and AcademicDegree fields marked as obsolete, but still can be used.
- Fix for issue #23: View controls was empty on repeating postbacks.
- Fix for issue #35: ViewEmployeeDetails crashes with "Value cannot be undefined" for format argument in the string.Format() method.
- Allow select division schedule from the list (like an employee shedule).
- Division module now searchable (preview, not all data exposed).
- MSBuild is used to create install and source packages instead of Package.cmd.
- Added suffix field for occupied positions to solve problems like then division is not explicitly defined.
- Employee's occupied positions now also editable.
- Delete occupied position and achievement commands now have confirmations.
- Careful update of occupied position (and achievement) records - don't delete / recreate all records in the DB.
- Modules now use default icon theme through IconController.
- Launchpad module will load to viewstate only tables which is about to display.
- Other small optimizations, improvements and fixes.

## Version 1.0.7

- Solution for global issue #17: Employee and EmployeeList modules now use custom data cache.
- Added popup with details about employee to the Employee and EmployeeList modules.
- Added ModuleBase classes to simplify access to controller and settings to Employee and Division modules.
- Fix issue #19: Then editing division from division's tab, HomePage reset to main page.
- EditEmployee control moved to Employee module from Launchpad.
- Fix issue #20: You can now add new division right from Division module.
- Launchpad pager mode changed to simplify access to the last records.
- More friendly SearchByTerm link in Division module.
- Reworked short title display logic in Division module.
- Remove endline after END:VCARD in generated vCards.
- Other small fixes and improvements.

## Version 1.0.6

- Added Division module to display information about specified division.
- EditDivision control moved to Division module from Launchpad.
- Added VCard class to base library with partial vCard implementation.
- Added QuotedPrintable class to base library to encode/decode QP-strings.
- Added vCard export to Division and Employee modules (in edit mode).
- Show QR-code in Division module with vCard info.
- Use Skin.AddModuleMessage() in Utils.Message() which mess with a ViewState before.
- Added Tweaks.SqlDataProvider script with temporary DB tweaks to execute manually.
- Fix issue #9: Working hours TermID saved instead of term Name.
- Localization updated, more messages moved to SharedResources.resx.
- Other small fixed and improvements.

## Version 1.0.5

- Added basic search by employee data in Employee and EmployeeList modules.
- Possible fix for issue #5: EditEmployee crashes if photo file was deleted.
- Fix for issue #6: Now employee positions grouped by division.
- Fix wrong sort order of employee positions.
- Fix for issue #7: Allow more chars for phone number fields in a DB.
- Fix for issue #8: Click on "Add position" button no longer results in a crash in EditEmployee positions tab if no division was selected.
- Trim string values then updating records in all forms.
- EditEmployee: Allow select commonly used working hours from a list.
- EditEmployee: About tab moved to the right.
- EditEmployee: Always select tab 0 then open new form.
- EditEmployee: Add separate "Add prime position" button instead of button + checkbox.
- EditEmployee: Autoselect division in a tree then editing from EmployeeList module.
- Launchpad: Load data tables in the session if it is empty.
- EditEmployee, Launchpad: Fix dnnGridHeader class doesn't apply to th element of gridview. 
- Other small fixed and improvements.

## Version 1.0.4

- Photo thumbnails now generated with R7.ImageHandler.
- Added settings to photo thumbnail width to Employee and EmployeeList modules.
- Fix for issue #4: University_Positions.ShortTitle field length expanded to 64.

**Note:** R7.ImageHandler is not required if photo width settings in Employee and EmployeeList modules is not set - original photos are used instead.

## Version 1.0.3

- Make setting and realize ability to select tables to display in Launchpad module.
- Added page size option for Launchpad module.
- Removed text wrapping around photo and edit link in EmployeeList module.
- Ability to display secondary email and profile links in Employee and EmployeeList modules.
- Lauchpad now preserve selected tab between page loads using session variable.
- Ability to add new employee right from Employee module.
- Added package script.

## Version 1.0.2

- Fix for issue #1 - DnnFilePickerUploader misses it's state between postbacks.
- Fix for issue #2 - DivisionsHierarchy table function returns wrong results.
- Added missing binding parameter to treeview.
- Cut biography string to 16 chars max.

## Version 1.0.1

- Then edit employee positions, select division from a tree instead of combobox.
- Employee about / biography moved to a separate tab.
- Use richtext editor for employee about / biography.
- Some style changes in Employee module.
- Allow update employee if no occupied positions defined.

## Version 1.0.0

This is the initial release.
