# Development Summary

## Recent Changes

### Navbar Menu Items Styling Fix (2026-01-05)

**Issue:** Menu items in the navbar were not responding to responsive font size styles.

**Root Cause:** Incorrect `sx` prop syntax in MenuItem components. The code was using `sx={{ navBarMenuItemStyle }}` which creates an object literal with a property named `navBarMenuItemStyle`, instead of spreading the actual style object.

**Solution:** Changed the syntax from `sx={{ navBarMenuItemStyle }}` to `sx={navBarMenuItemStyle}` to properly apply the style object.

**Files Modified:**
- `client/src/app/layout/Navbar.tsx` (lines 61, 69, 77)

**Responsive Styles Applied:**
- The `navBarMenuItemStyle` from `client/src/app/shared/actionStyles.ts` now properly applies:
  - `xs` (extra small screens): `0.5rem` font size
  - `sm` (small screens): `0.875rem` font size
  - `md` (medium+ screens): `1rem` font size
  - `whiteSpace: 'nowrap'` to prevent text wrapping

**Result:** Menu items now properly shrink on smaller screens, improving mobile responsiveness.

---

### Submit Button Not Working in LocationsDashboard Dialog (2026-01-08)

**Issue:** Submit button in the "New location" dialog was not triggering form submission when clicked.

**Root Cause:** The submit button with `type="submit"` was placed outside the `<form>` element. The form element was only wrapping the input field inside `DialogContent`, but the submit button was in `DialogActions` which was outside the form boundaries. HTML submit buttons only work when they're inside the form element they're meant to submit.

**Solution:** Moved the form wrapper (`<Box component="form" onSubmit={...}>`) to wrap both `DialogContent` and `DialogActions`, so the submit button is now inside the form element.

**Files Modified:**
- `client/src/app/features/Locations/LocationsDashboard.tsx` (lines 175-197)

**Code Structure Change:**
```tsx
// Before (broken):
<Dialog>
  <DialogContent>
    <Box component="form">  {/* Form only wraps input */}
      <TextInput />
    </Box>
  </DialogContent>
  <DialogActions>
    <Button type="submit">Submit</Button>  {/* Outside form! */}
  </DialogActions>
</Dialog>

// After (fixed):
<Dialog>
  <Box component="form">  {/* Form wraps both sections */}
    <DialogContent>
      <TextInput />
    </DialogContent>
    <DialogActions>
      <Button type="submit">Submit</Button>  {/* Inside form! */}
    </DialogActions>
  </Box>
</Dialog>
```

**Result:** Submit button now properly triggers the form's `onSubmit` handler, calling `OnSubmitDialogue` function which adds the new location.

---

### Edit Button Size Not Matching Small Icon (2026-01-12)

**Issue:** The edit button (Fab component) on line 177 maintained its default "small" size even though the icon inside it was set to be smaller with `fontSize="inherit"`. The button didn't shrink to match the smaller icon, creating a visual imbalance.

**Root Cause:** MUI's Fab component with `size="small"` has a fixed default size (~40x40 pixels). Using `fontSize="inherit"` on the icon makes the icon smaller, but doesn't affect the button's dimensions. The Fab component doesn't automatically adjust its size based on icon size.

**Solution:** Added custom `sx` prop to override the Fab button dimensions and changed icon fontSize to be explicit.

**Files Modified:**
- `client/src/app/features/Locations/LocationsDashboard.tsx` (lines 174-181)

**Changes Made:**
```tsx
// Added sx prop to Fab button
sx={{
  width: 32,
  height: 32,
  minHeight: 32,
  minWidth: 32,
}}
// Changed icon fontSize from "inherit" to "small"
<ModeEditIcon fontSize="small" />
```

**New CSS/Styling Technique:**
- Using MUI's `sx` prop with explicit width/height dimensions to override default Fab button sizing
- Custom dimensions (32x32px) to match small icon size, instead of relying on the default "small" size (~40x40px)
- Setting both `width`/`height` and `minWidth`/`minHeight` ensures the button doesn't expand beyond desired size

**Result:** Edit button now properly shrinks to match the smaller icon inside it, creating better visual proportion and consistency in the UI.

---

### Edit Mode State Always False and TypeScript Compilation Error (2026-01-12)

**Issue 1:** `isEditMode` state was always `false` even after clicking the edit button (`handleEditButtonClick`).

**Root Cause:** Event bubbling caused the click event to propagate from the edit IconButton to its parent TableRow. The sequence was:
1. Click edit button → `handleEditButtonClick` sets `isEditMode` to `true`
2. Event bubbles to TableRow → `handleRowSelected` sets `isEditMode` back to `false`

**Issue 2:** TypeScript compilation error on line 224 due to incorrect event type.

**Root Cause:** The `handleEditButtonClick` function was typed with `MouseEvent` (native DOM type) instead of `React.MouseEvent<HTMLButtonElement>` (React's synthetic event type), causing a type mismatch when passed to the IconButton's `onClick` handler.

**Solution:**
1. Added `event.stopPropagation()` to prevent event bubbling to parent TableRow
2. Fixed TypeScript typing from `MouseEvent` to `React.MouseEvent<HTMLButtonElement>`

**Files Modified:**
- `client/src/app/features/Locations/LocationsDashboard.tsx` (lines 77-80, 223-225)

**Changes Made:**
```tsx
// Before (broken):
const handleEditButtonClick = (event: MouseEvent) => {
  setIsEditMode(true);
  console.log(IsEditMode);
};

// After (fixed):
const handleEditButtonClick = (event: React.MouseEvent<HTMLButtonElement>) => {
  event.stopPropagation();
  setIsEditMode(true);
};

// IconButton now properly passes event:
<IconButton onClick={(event) => { handleEditButtonClick(event); }}>
```

**Key Concepts:**
- **Event Bubbling:** Events in React (and DOM) propagate from the target element up through parent elements. Use `event.stopPropagation()` to prevent this when child and parent have conflicting click handlers.
- **React Synthetic Events:** React wraps native browser events with `React.MouseEvent`, `React.ChangeEvent`, etc. These types require generic parameters like `<HTMLButtonElement>` to specify the element type.

**Result:**
1. Edit mode now correctly activates when clicking the edit button without being reset by parent row click
2. TypeScript compilation error resolved with proper React event typing

---

### Form Wrapping TableRow Causing Hydration Error (2026-01-13)

**Issue:** React hydration error: "In HTML, <tr> cannot be a child of <form>. This will cause a hydration error."

**Root Cause:** The form element (Box with `component="form"`) was wrapping the entire TableRow, creating invalid HTML structure. In HTML, `<form>` elements cannot be between `<tbody>` and `<tr>`, resulting in:
```html
<tbody>
  <form>  <!-- Invalid! -->
    <tr>...</tr>
  </form>
</tbody>
```

**Solution:** Restructured the code to place the form inside the TableCell instead of wrapping the TableRow. Used HTML's `form` attribute to connect the submit button (in a different cell) to the form by id.

**Files Modified:**
- `client/src/app/features/Locations/LocationsDashboard.tsx` (lines 163-247)

**Changes Made:**
```tsx
// Before (invalid HTML):
<Box component="form" display={"contents"} onSubmit={...}>
  <TableRow>
    <TableCell>{/* input here */}</TableCell>
    <TableCell>{/* button here */}</TableCell>
  </TableRow>
</Box>

// After (valid HTML):
const formId = `edit-form-${locationFromServer.id}`;
<TableRow>
  <TableCell>
    <Box component="form" id={formId} onSubmit={...}>
      {/* input here */}
    </Box>
  </TableCell>
  <TableCell>
    <IconButton type="submit" form={formId}>
      {/* button references form by id */}
    </IconButton>
  </TableCell>
</TableRow>
```

**New HTML Technique:**
- **form attribute on submit buttons:** HTML allows buttons to be associated with forms outside their DOM hierarchy using the `form` attribute. The button's `form="formId"` connects it to a form with `id="formId"` anywhere in the document.
- This enables proper form submission even when the button can't be a direct child of the form due to HTML structure constraints (like table layouts).

**Result:** Valid HTML structure that eliminates hydration error while maintaining form submission functionality. Each row's form has a unique id, and the save button correctly submits its corresponding form.

---

### Form Submitting Old Values Instead of Updated Values (2026-01-13)

**Issue:** When clicking the save button after editing a location name, the form was submitting the old values instead of the newly entered values.

**Root Cause:** Event bubbling from the save button. The click sequence was:
1. User clicks save button → Form prepares to submit with new values
2. Click event bubbles to TableRow → `handleRowSelected` is triggered
3. `handleRowSelected` calls `resetUpdate(selectedLocation)` → Form is reset to old values
4. Form submits with the reset (old) values instead of the user's input

**Solution:** Added `event.stopPropagation()` to the save button's onClick handler to prevent the click event from bubbling to the parent TableRow. Also added `setIsEditMode(false)` after successful save to exit edit mode.

**Files Modified:**
- `client/src/app/features/Locations/LocationsDashboard.tsx` (lines 81-88, 235-246)

**Changes Made:**
```tsx
// Added stopPropagation to save button:
<IconButton
  type="submit"
  form={formId}
  onClick={(event) => {
    event.stopPropagation();  // Prevent bubbling to TableRow
  }}
>
  <SaveIcon />
</IconButton>

// Added exit from edit mode after successful save:
const handleUpdateLocation = async (location: CategorySchema) => {
  const dataToSend = { ...SelectedLocationTableItem, ...location } as unknown as StorageLocation;
  await updateLocation.mutateAsync(dataToSend);
  setIsEditMode(false);  // Exit edit mode after save
};
```

**Key Concept:**
This is the same event bubbling issue as the edit button, but more critical because the bubbling triggers a form reset that interferes with form submission. Both action buttons (edit and save) within table rows need `stopPropagation()` to prevent interference from the row's click handler.

**Result:** Form now correctly submits the updated values entered by the user, and automatically exits edit mode after successful save.

---
