import { useState } from "react";
import DeleteIcon from "@mui/icons-material/Delete";
import ModeEditIcon from "@mui/icons-material/ModeEdit";
import AddIcon from "@mui/icons-material/Add";
import useLocations from "../../../lib/hooks/store/useLocations";
import SaveIcon from "@mui/icons-material/Save";

import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Divider,
  Fab,
  IconButton,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from "@mui/material";
import { StorageLocation } from "../../../lib/types/storage";
import { useForm } from "react-hook-form";
import {
  categorySchema,
  CategorySchema,
} from "../../../lib/schemas/categorySchema";
import { zodResolver } from "@hookform/resolvers/zod";
import TextInput from "../../shared/components/TextInput";

export default function LocationsDashboard() {
  const {
    deleteLocation,
    isGettingLocationsPending,
    locationsFromServer,
    updateLocation,
    addLocation,
  } = useLocations();
  // this will probably change to a common schema between all objects
  const { handleSubmit, reset, control } = useForm<CategorySchema>({
    mode: "onTouched",
    resolver: zodResolver(categorySchema),
  });

  const {
    handleSubmit: handleSubmitUpdate,
    reset: resetUpdate,
    control: controlUpdate,
    formState: updateformstate,
  } = useForm<CategorySchema>({
    mode: "onTouched",
    resolver: zodResolver(categorySchema),
  });

  const [SelectedLocationTableItem, setSelectedLocationTableItem] =
    useState<StorageLocation>();

  // triggers the edit on table rows
  const [IsEditMode, setIsEditMode] = useState<boolean>(false);

  //handles opening the add popup
  const [IsAddMode, setIsAddMode] = useState<boolean>(false);

  const handleRowSelected = (selectedLocation: StorageLocation) => {
    setSelectedLocationTableItem(selectedLocation);

    resetUpdate(selectedLocation);

    //put this back to false to not trigger the input text on row change
    setIsEditMode(false);
  };

  const handleDeleteLocation = async (idLocation: number) => {
    await deleteLocation.mutateAsync(idLocation);
  };

  const handleUpdateLocation = async (location: CategorySchema) => {
    const dataToSend = {
      ...SelectedLocationTableItem,
      ...location,
    } as unknown as StorageLocation;
    await updateLocation.mutateAsync(dataToSend);
    setIsEditMode(false);
  };

  const handleEditButtonClick = (
    event: React.MouseEvent<HTMLButtonElement>
  ) => {
    event.stopPropagation();
    setIsEditMode(true);
  };

  const OnSubmitDialogueAdd = async (schema: CategorySchema) => {
    const dataToSend = { ...schema } as StorageLocation;
    await addLocation.mutateAsync(dataToSend as unknown as StorageLocation);
    handleDialogueClose();
  };
  const handleDialogueClose = () => {
    reset();
    setIsAddMode(false);
  };
  if (isGettingLocationsPending) {
    return <div>Loading...</div>;
  }

  return (
    <Box>
      <Paper
        sx={{
          width: "100%",
          p: 2,
          overflow: "hidden",
          display: "flex",
          flexDirection: "column",
          height: 650,
        }}
      >
        {!locationsFromServer ? (
          <Box>No data</Box>
        ) : (
          <Box>
            <TableContainer component={Paper}>
              <Table stickyHeader sx={{ minWidth: 100 }}>
                <TableHead>
                  <TableRow>
                    <TableCell>Name</TableCell>
                    <TableCell>
                      <Box
                        display={"flex"}
                        alignContent={"end"}
                        justifyContent={"flex-end"}
                        // maxWidth={{ xs: 150 }}
                        gap={1}
                      >
                        <Fab
                          size="small"
                          color="success"
                          onClick={() => {
                            setIsAddMode(true);
                          }}
                        >
                          <AddIcon />
                        </Fab>

                        <Fab
                          size="small"
                          color="error"
                          disabled={
                            !SelectedLocationTableItem ||
                            SelectedLocationTableItem!.hasChildren
                          }
                          onClick={() =>
                            handleDeleteLocation(SelectedLocationTableItem!.id)
                          }
                        >
                          <DeleteIcon />
                        </Fab>
                      </Box>
                    </TableCell>
                  </TableRow>
                </TableHead>
                <TableBody>
                  {locationsFromServer.map((locationFromServer) => {
                    const isrowSelected =
                      locationFromServer == SelectedLocationTableItem;
                    const formId = `edit-form-${locationFromServer.id}`;

                    return (
                      <TableRow
                        hover
                        selected={isrowSelected}
                        onClick={() => {
                          handleRowSelected(locationFromServer);
                        }}
                        role="checkbox"
                        key={locationFromServer.id}
                      >
                        <TableCell>
                          <Box
                            component="form"
                            id={formId}
                            display={"inline-flex"}
                            flexDirection={"row"}
                            gap={1}
                            onSubmit={handleSubmitUpdate(handleUpdateLocation)}
                          >
                            <Box
                              alignSelf={"center"}
                              justifyContent={"flex-start"}
                            >
                              {!(IsEditMode && isrowSelected) ? (
                                locationFromServer.name
                              ) : (
                                <TextInput
                                  control={controlUpdate}
                                  autoFocus
                                  required
                                  margin="dense"
                                  name="name"
                                  label="Location"
                                  type="text"
                                  fullWidth
                                  variant="standard"
                                />
                              )}
                            </Box>
                          </Box>
                        </TableCell>

                        <TableCell>
                          <Box
                            visibility={isrowSelected ? "inherit" : "collapse"}
                            display={"flex"}
                            justifyContent={"flex-end"}
                            flexDirection={"row"}
                            gap={1}
                          >
                            <Divider orientation="vertical" flexItem />

                            <IconButton
                              size="small"
                              color="primary"
                              disabled={
                                !SelectedLocationTableItem || !isrowSelected
                              }
                              onClick={(event) => {
                                handleEditButtonClick(event);
                              }}
                            >
                              <ModeEditIcon />
                            </IconButton>
                            <IconButton
                              size="small"
                              color="primary"
                              type="submit"
                              form={formId}
                              disabled={!updateformstate.isDirty}
                              onClick={(event) => {
                                event.stopPropagation();
                              }}
                            >
                              <SaveIcon />
                            </IconButton>
                          </Box>
                        </TableCell>
                      </TableRow>
                    );
                  })}
                </TableBody>
              </Table>
            </TableContainer>
          </Box>
        )}
      </Paper>
      <Dialog open={IsAddMode} onClose={handleDialogueClose}>
        <Box
          component="form"
          key={"AddLocation"}
          onSubmit={handleSubmit(OnSubmitDialogueAdd)}
        >
          <DialogTitle>New location</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Enter new storage location name
            </DialogContentText>
            <TextInput
              control={control}
              autoFocus
              required
              margin="dense"
              name="name"
              label="Location"
              type="text"
              fullWidth
              variant="standard"
            />
          </DialogContent>
          <DialogActions>
            <Button onClick={handleDialogueClose}>Cancel</Button>
            <Button type="submit">Submit</Button>
          </DialogActions>
        </Box>
      </Dialog>
    </Box>
  );
}
