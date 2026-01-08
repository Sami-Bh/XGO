import { useState } from "react";
import DeleteIcon from "@mui/icons-material/Delete";
import ModeEditIcon from "@mui/icons-material/ModeEdit";
import AddIcon from "@mui/icons-material/Add";
import useLocations from "../../../lib/hooks/store/useLocations";
import {
  Box,
  Button,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  Fab,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  TextField,
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

  const [SelectedLocationTableItem, setSelectedLocationTableItem] =
    useState<StorageLocation>();

  const [IsEditMode, setIsEditMode] = useState<boolean>(false);

  //handles opening the add popup
  const [IsAddMode, setIsAddMode] = useState<boolean>(false);

  const handleRowSelected = (selectedLocation: StorageLocation) => {
    setSelectedLocationTableItem(selectedLocation);
  };

  const handleDeleteLocation = async (idLocation: number) => {
    await deleteLocation.mutateAsync(idLocation);
  };

  const handleUpdateLocation = async (location: StorageLocation) => {
    await updateLocation.mutateAsync(location);
  };

  // this will probably change to a common schema between all objects
  const { handleSubmit, reset, control } = useForm<CategorySchema>({
    mode: "onTouched",
    resolver: zodResolver(categorySchema),
  });

  const OnSubmitDialogue = async (schema: CategorySchema) => {
    const dataToSend = { ...schema };
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
  console.log(locationsFromServer);

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
                          color="primary"
                          disabled={!SelectedLocationTableItem}
                          onClick={() => {}}
                        >
                          <ModeEditIcon />
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
                        <TableCell colSpan={2}>
                          {!IsEditMode ? (
                            locationFromServer.name
                          ) : (
                            <TextField />
                          )}
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
        <Box component="form" onSubmit={handleSubmit(OnSubmitDialogue)}>
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
