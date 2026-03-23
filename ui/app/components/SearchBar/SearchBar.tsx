import { useEffect, useState } from "react"

export default function SearchBar({ storageKey, onSearch, searchTip }: SearchBarProps) {
    const [query, setQuery] = useState("")
    const [history, setHistory] = useState<string[]>([])
    const [showDropdown, setShowDropdown] = useState(false)

    const historyItems = history.filter(item => item.includes(query))

    useEffect(() => {
        const stored = localStorage.getItem(storageKey)
        if (stored) {
            setHistory(JSON.parse(stored))
        }
    }, [])

    function saveHistory(newHistory: string[]) {
        setHistory(newHistory)
        localStorage.setItem(storageKey, JSON.stringify(newHistory))
    }

    function handleKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
        if (e.key === "Enter") {
            handleSearch(query)
        }
    }

    async function handleSearch(value: string) {
        if (!value.trim()) {
            return
        }

        const success = await onSearch(value)
        if (!success) {
            return
        }

        const newHistory = [
            value,
            ...history.filter(h => h !== value)
        ].slice(0, 10)

        saveHistory(newHistory)
    }

  return (
      <div className="relative w-full max-w-md">
        <input
          type="text"
          value={query}
          onChange={(e) => setQuery(e.target.value)}
          onFocus={() => setShowDropdown(true)}
          onBlur={() => setTimeout(() => setShowDropdown(false), 100)}
          onKeyDown={handleKeyDown}
          placeholder={searchTip}
          className="w-full p-3 border rounded-lg"
        />

        {showDropdown && query && historyItems.length > 0 && (
          <div className="absolute top-full mt-1 w-full bg-white border rounded-lg shadow">
            {historyItems.map((item, i) => (
              <div
                key={i}
                onClick={() => {
                  setQuery(item)
                  handleSearch(item)
                }}
                className="p-2 hover:bg-gray-100 cursor-pointer text-black"
              >
                {item}
              </div>
            ))}
          </div>
        )}
      </div>
    )
}