import SearchBar from "../SearchBar/SearchBar";

export default function HomePage() {

    const handleSearch = async (search: string) => {
        
        return false
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