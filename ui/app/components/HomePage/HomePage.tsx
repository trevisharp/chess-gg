import { useNavigate } from "react-router";
import SearchBar from "../SearchBar/SearchBar";

export default function HomePage() {

    const navigate = useNavigate()

    const handleSearch = async (search: string) => {
        navigate("/analysis/" + search)
        return true
    }

    return (
        <div className="h-full w-full flex items-center justify-center">
            <SearchBar
                storageKey="player-history"
                searchTip="Search by chess.com username..."
                onSearch={handleSearch}
            />
        </div>
    )
}