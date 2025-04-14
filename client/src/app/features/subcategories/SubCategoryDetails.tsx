import { Box, Paper, Card, CardHeader, CardContent, Button, Typography } from '@mui/material'
import { useNavigate, useParams } from 'react-router'
import { useForm } from 'react-hook-form';
import { subcategorySchema, SubcategorySchema } from '../../../lib/schemas/subcategorySchema';
import { useEffect } from 'react';
import { zodResolver } from '@hookform/resolvers/zod';
import TextInput from '../../shared/components/TextInput';
import { subcategoriesUri } from '../../routes/routesconsts';
import useSubCategory from '../../../lib/hooks/store/useSubCategory';

export default function SubCategoryDetails() {
    const { categoryId, id } = useParams();

    const { isGetSubCategoryLoading, subcategoryFromServer,
        createNewSubCategory, updateSubCategory,
        deleteSubCategory
    } = useSubCategory(Number(categoryId), Number(id));

    const navigate = useNavigate();
    const { handleSubmit, reset, control } = useForm<SubcategorySchema>({
        mode: 'onTouched',
        resolver: zodResolver(subcategorySchema)
    });

    const buttonName = subcategoryFromServer ? "Edit" : "Create";
    const OnSubmitForm = async (data: SubcategorySchema) => {
        const objectToSend = { ...subcategoryFromServer, ...data, categoryId: categoryId };

        if (subcategoryFromServer) {
            await updateSubCategory.mutateAsync(objectToSend as unknown as SubCategory, {
                onSuccess: () => {
                    goBackToDashBoard();
                }
            });
        }
        else {
            await createNewSubCategory.mutateAsync(objectToSend as unknown as SubCategory, {
                onSuccess: (id) => {
                    navigate(`${subcategoriesUri}/${categoryId}/${id}`);
                }
            });
        }
    }
    const handleDelete = async () => {
        await deleteSubCategory.mutateAsync(Number(id), {
            onSuccess: () => {
                goBackToDashBoard();
            }
        });

    }
    const goBackToDashBoard = () => {
        navigate(`${subcategoriesUri}/${categoryId}`);
    }



    useEffect(() => {
        if (subcategoryFromServer) reset(subcategoryFromServer);
    }, [subcategoryFromServer, reset]);


    if (isGetSubCategoryLoading) {
        return <Typography variant='h3'> Loading ...</Typography>
    }

    return (
        <Box
            component="form" key={subcategoryFromServer?.id}
            onSubmit={handleSubmit(OnSubmitForm)}
        >
            <Paper>
                <Card>
                    <CardHeader title="Sub Category Details" slotProps={{
                        title: { fontWeight: "bold", fontSize: 20 }
                    }} />
                    <CardContent>
                        <TextInput name='name' control={control} label="Name" />

                    </CardContent>

                    <CardContent>
                        <Box sx={{ display: "flex", justifyContent: "flex-end", gap: 1 }}>
                            {!!subcategoryFromServer && <Button disabled={subcategoryFromServer.hasChildren} onClick={() => handleDelete()} variant="contained" size="small" color="error">Delete</Button>}
                            {!subcategoryFromServer && <Button onClick={() => goBackToDashBoard()} variant="contained" size="small" color="inherit">Cancel</Button>}
                            <Button disabled={createNewSubCategory.isPending || updateSubCategory.isPending} type='submit' variant="contained" size="small" color="primary">
                                {buttonName}
                            </Button>
                        </Box>
                    </CardContent>
                </Card>
            </Paper>

        </Box>
    )
}
