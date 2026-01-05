// Reusable responsive font styles for action components
export const actionIconFontSize = {
  xs: "1.2rem",
  sm: "1.5rem",
  md: "1.75rem",
};

export const actionTitleFontSize = {
  xs: "1rem",
  sm: "1.25rem",
  md: "1.5rem",
};

export const actionButtonFontSize = {
  xs: "0.5rem",
  sm: "0.875rem",
  md: "1rem",
};

export const actionButtonIconSize = {
  xs: "1rem",
  sm: "1.25rem",
};

// Reusable Typography style for action headers
export const actionTitleStyle = {
  alignSelf: "center",
  fontSize: actionTitleFontSize,
  display: "flex",
  alignItems: "center",
  gap: 0.5,
};

export const actionsBoxStyle = {
  display: "flex",
  flexDirection: "column",
  mx: { xs: 2, sm: 3, md: 5 },
  pb: 1,
  gap: 2,
};

// Reusable styles for filter components
export const filterSelectStyle = {
  fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
  "& .MuiInputLabel-root": {
    fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
  },
  "& .MuiSelect-select": {
    fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
  },
};

export const filterTextFieldStyle = {
  "& .MuiInputLabel-root": {
    fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
  },
  "& .MuiInput-input": {
    fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
  },
};

export const filterButtonStyle = {
  fontSize: { xs: "0.75rem", sm: "0.75rem", md: "0.75rem" },
  py: { xs: 1, sm: 1.5 },
};

export const quantityFieldStyle = {
  fontSize: { xs: "0.5rem", sm: "0.875rem", md: "1rem" },
  width: { xs: 60, sm: 60, md: 60 }
};

export const navBarItemBaseStyle = {
  whiteSpace: 'nowrap',
  fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },

};
export const navBarMenuItemStyle = {
  ...navBarItemBaseStyle,
  fontSize: { xs: "0.75rem", sm: "0.875rem", md: "1rem" },
};
