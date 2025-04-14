import { useState } from "react"
import { StorageFilter } from "../../../lib/types/storage"
import useStorageItems from "../../../lib/hooks/useStorageItems";

export default function StorageTable() {
    const getDefaultFilter = (): StorageFilter => { return { oderDirection: "asc", orderby: "name", pageIndex: 1, pageSize: 5, productNameSearchText: "", StorageId: 1 } };
    const [Filter, setFilter] = useState<StorageFilter>(getDefaultFilter());
    const { StoredItemsFromServer, IsStoredItemsFromServerPending } = useStorageItems(Filter);
    return (
        <div>StorageTable</div>
    )
}
