import { Box, Button, Card, CardContent, CardHeader, Paper, Typography } from '@mui/material'
import { useEffect } from 'react';
import { observer } from "mobx-react-lite"
import { useForm } from 'react-hook-form';
import { useNavigate, useParams } from 'react-router';
import { zodResolver } from "@hookform/resolvers/zod"
import { CategorySchema, categorySchema } from '../../../lib/schemas/categorySchema';
import { categoriesUri, } from '../../routes/routesconsts';
import TextInput from '../../shared/components/TextInput';
import useCategories from '../../../lib/hooks/store/useCategories';

const CategoryDetails = observer(function CategoryDetails() {
    const { id: selectedCategoryId } = useParams();
    const navigate = useNavigate();
    const { handleSubmit, reset, control } = useForm<CategorySchema>({
        mode: 'onTouched',
        resolver: zodResolver(categorySchema)
    });
    const { categoryFromServer, isGettingCategoryLoading, updateCategory, deleteCategory, createCategory } = useCategories(Number(selectedCategoryId));
    const category = categoryFromServer;
    const buttonName = selectedCategoryId ? "Edit" : "Create";

    const goBackToDashBoard = () => {
        navigate(categoriesUri);

    }

    useEffect(() => {
        if (category) { reset(category); }
    }, [category, reset]);

    const OnSubmitForm = async (data: CategorySchema) => {
        const dataToSend = { ...category, ...data, };
        if (selectedCategoryId) {
            await updateCategory.mutateAsync(dataToSend as unknown as Category, {
                onSuccess: () => { goBackToDashBoard(); }
            });

        } else {
            await createCategory.mutateAsync(dataToSend as unknown as Category, {
                onSuccess: (id) => {
                    navigate(`${categoriesUri}/${id}`);

                }
            });
        }
    }

    const handleDelete = async () => {

        await deleteCategory.mutateAsync(Number(selectedCategoryId), {
            onSuccess: () => { goBackToDashBoard(); }
        });
    }

    // if (!categorystore.isEditMode()) return (<></>)
    if (selectedCategoryId && isGettingCategoryLoading) return (<Typography variant="h3">Loading...</Typography>)

    return (
        <Box

            component="form" key={selectedCategoryId} onSubmit={handleSubmit(OnSubmitForm)}>
            <Paper>
                <Card>
                    <CardHeader title="Category Details" slotProps={{
                        title: { fontWeight: "bold", fontSize: 20 }
                    }} />
                    <CardContent>
                        <TextInput label="Name" name='name' control={control} />

                    </CardContent>

                    <CardContent>
                        <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 1 }}>
                            {!!category && <Button disabled={category.hasChildren} onClick={() => handleDelete()} variant="contained" size="small" color="error">Delete</Button>}
                            {!category && <Button onClick={() => goBackToDashBoard()} variant="contained" size="small" color="inherit">Cancel</Button>}
                            <Button type='submit' variant="contained" size="small" color="primary">
                                {buttonName}
                            </Button>
                        </Box>
                    </CardContent>
                </Card>
            </Paper>

        </Box>
    )
});

export default CategoryDetails;

